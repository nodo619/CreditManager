Ext.define('CreditManager.UI.service.AuthService', {
    singleton: true,

    config: {
        identityServerUrl: 'https://localhost:5001',
        clientId: 'creditmanager.ui',
        redirectUri: 'http://localhost:1841/signin-oidc.html',
        responseType: 'code',
        scope: 'openid profile email roles creditmanager.api offline_access userinfo'
    },

    constructor: function(config) {
        this.initConfig(config);
        this.checkForAuthCode();
        return this;
    },

    checkForAuthCode: function() {
        var params = new URLSearchParams(window.location.search);
        var code = params.get('code');
        var error = params.get('error');
        var errorDescription = params.get('error_description');

        if (code) {
            console.log('Authorization code found in URL, initiating token exchange...');
            this.exchangeCodeForToken(code);
        } else if (error) {
            console.error('Error in authorization:', error, errorDescription);
            Ext.Msg.alert('Error', errorDescription || error);
        }
    },

    // Check if user is authenticated
    isAuthenticated: function() {
        return !!this.getToken();
    },

    // Get the current token
    getToken: function() {
        return localStorage.getItem('access_token');
    },

    // Start the login process
    login: function() {
        var me = this;
        // Generate PKCE values
        var codeVerifier = this.generateCodeVerifier();
        
        console.log('Generated code verifier:', codeVerifier);
        
        // Store code verifier for later use
        localStorage.setItem('code_verifier', codeVerifier);
        
        // Generate code challenge
        this.generateCodeChallenge(codeVerifier).then(function(codeChallenge) {
            console.log('Generated code challenge:', codeChallenge);
            
            // Build authorization URL
            var authUrl = 'https://localhost:5001/connect/authorize?' + Ext.Object.toQueryString({
                client_id: 'creditmanager.ui',
                redirect_uri: 'http://localhost:1841/signin-oidc.html',
                response_type: 'code',
                scope: 'openid profile email roles creditmanager.api offline_access',
                code_challenge: codeChallenge,
                code_challenge_method: 'S256',
                state: me.generateState()
            });

            console.log('Authorization URL:', authUrl);
            window.location.href = authUrl;
        });
    },

    // Handle the callback from Identity Server
    handleCallback: function() {
        var me = this;
        console.log('Starting callback handling...');

        try {
            var params = new URLSearchParams(window.location.search);
            var code = params.get('code');
            var error = params.get('error');
            var errorDescription = params.get('error_description');
            var state = params.get('state');

            console.log('Callback received:', {
                code: code ? 'present' : 'missing',
                error: error,
                errorDescription: errorDescription,
                state: state
            });

            if (error) {
                console.error('Error in callback:', error, errorDescription);
                Ext.Msg.alert('Error', errorDescription || error);
                return;
            }

            if (!code) {
                console.error('No authorization code received');
                Ext.Msg.alert('Error', 'No authorization code received');
                return;
            }

            // Get the stored code verifier
            var codeVerifier = localStorage.getItem('code_verifier');
            console.log('Stored code verifier:', codeVerifier ? 'present' : 'missing');

            if (!codeVerifier) {
                console.error('Code verifier not found in localStorage');
                Ext.Msg.alert('Error', 'Code verifier not found');
                return;
            }

            // Exchange the code for tokens
            console.log('Initiating token exchange...');
            this.exchangeCodeForToken(code);
        } catch (error) {
            console.error('Error in handleCallback:', error);
            sessionStorage.removeItem('code_processed');
            localStorage.removeItem('code_verifier');
            Ext.Msg.alert('Error', 'Failed to process login: ' + error.message);
        }
    },

    // Exchange authorization code for tokens
    exchangeCodeForToken: function(code) {
        var me = this;
        console.log('Starting token exchange...');

        try {
            var codeVerifier = localStorage.getItem('code_verifier');
            
            console.log('Preparing token request...');
            var params = new URLSearchParams();
            params.append('grant_type', 'authorization_code');
            params.append('client_id', 'creditmanager.ui');
            params.append('code', code);
            params.append('redirect_uri', 'http://localhost:1841/signin-oidc.html');
            params.append('code_verifier', codeVerifier);

            var requestParams = params.toString();
            console.log('Token request params:', requestParams);

            console.log('Sending token request to:', 'https://localhost:5001/connect/token');
            Ext.Ajax.request({
                url: 'https://localhost:5001/connect/token',
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'Accept': 'application/json'
                },
                cors: true,
                useDefaultXhrHeader: false,
                params: requestParams,
                success: function(response) {
                    console.log('Token response received:', response);
                    try {
                        var tokens = Ext.decode(response.responseText);
                        console.log('Tokens decoded successfully:', {
                            access_token: tokens.access_token ? 'present' : 'missing',
                            id_token: tokens.id_token ? 'present' : 'missing',
                            refresh_token: tokens.refresh_token ? 'present' : 'missing'
                        });
                        me.saveTokens(tokens);
                        // Clear the code verifier after successful token exchange
                        localStorage.removeItem('code_verifier');
                        // Clear the session storage flag
                        sessionStorage.removeItem('code_processed');
                        // Redirect to the main app
                        console.log('Redirecting to main app...');
                        window.location.replace('/');
                    } catch (error) {
                        console.error('Error processing token response:', error);
                        console.error('Response text:', response.responseText);
                        sessionStorage.removeItem('code_processed');
                        localStorage.removeItem('code_verifier');
                        Ext.Msg.alert('Error', 'Failed to process login response: ' + error.message);
                    }
                },
                failure: function(response) {
                    console.error('Token request failed:', response);
                    console.error('Response status:', response.status);
                    console.error('Response status text:', response.statusText);
                    console.error('Response text:', response.responseText);
                    var error = response.responseText ? Ext.decode(response.responseText) : { error_description: 'Unknown error occurred' };
                    console.error('Error details:', error);
                    // Clear the session storage flag on failure
                    sessionStorage.removeItem('code_processed');
                    localStorage.removeItem('code_verifier');
                    Ext.Msg.alert('Error', error.error_description || 'Failed to exchange code for token');
                }
            });
        } catch (error) {
            console.error('Error in exchangeCodeForToken:', error);
            sessionStorage.removeItem('code_processed');
            localStorage.removeItem('code_verifier');
            Ext.Msg.alert('Error', 'Failed to exchange code for token: ' + error.message);
        }
    },

    // Save tokens to local storage
    saveTokens: function(tokens) {
        console.log('Saving tokens...');
        if (!tokens.access_token) {
            console.error('No access token in response');
            throw new Error('No access token received');
        }
        localStorage.setItem('access_token', tokens.access_token);
        localStorage.setItem('id_token', tokens.id_token);
        if (tokens.refresh_token) {
            localStorage.setItem('refresh_token', tokens.refresh_token);
        }
        console.log('Tokens saved successfully');
    },

    // Get user information from ID token
    getUserInfo: function() {
        const token = localStorage.getItem('access_token');
        if (!token) {
            console.log('No access token found');
            return null;
        }

        try {
            const decodedToken = this.parseJwt(token);
            console.log('Decoded token:', decodedToken);
            
            const userInfo = {
                id: decodedToken.sub,
                email: decodedToken.email,
                firstName: decodedToken.first_name,
                lastName: decodedToken.last_name,
                name: decodedToken.name,
                role: decodedToken.role
            };
            
            console.log('Extracted user info:', userInfo);
            return userInfo;
        } catch (error) {
            console.error('Error parsing token:', error);
            return null;
        }
    },

    // Parse JWT token
    parseJwt: function(token) {
        try {
            console.log('Parsing JWT token');
            var base64Url = token.split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            var decoded = JSON.parse(jsonPayload);
            console.log('Successfully parsed JWT:', decoded);
            return decoded;
        } catch (error) {
            console.error('Error parsing JWT:', error);
            return null;
        }
    },

    // Logout
    logout: function() {
        var me = this;
        var idToken = localStorage.getItem('id_token');
        localStorage.removeItem('access_token');
        localStorage.removeItem('id_token');
        localStorage.removeItem('refresh_token');
        localStorage.removeItem('code_verifier');
        
        var logoutUrl = me.getIdentityServerUrl() + '/connect/endsession?' + Ext.Object.toQueryString({
            id_token_hint: idToken,
            post_logout_redirect_uri: me.getRedirectUri().replace('signin-oidc', 'signout-callback-oidc')
        });
        
        window.location.href = logoutUrl;
    },

    // Get URL parameter
    getUrlParameter: function(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    },

    // Generate state parameter for security
    generateState: function() {
        return Ext.id();
    },

    // Generate nonce for security
    generateNonce: function() {
        return Ext.id();
    },

    // Generate a random code verifier for PKCE
    generateCodeVerifier: function() {
        var array = new Uint8Array(32);
        window.crypto.getRandomValues(array);
        var verifier = Array.from(array, function(byte) {
            return ('0' + (byte & 0xFF).toString(16)).slice(-2);
        }).join('');
        return this.base64URLEncode(verifier);
    },

    // Generate code challenge from verifier
    generateCodeChallenge: function(verifier) {
        var encoder = new TextEncoder();
        var data = encoder.encode(verifier);
        return window.crypto.subtle.digest('SHA-256', data)
            .then(function(hash) {
                var hashArray = new Uint8Array(hash);
                var hashBase64 = btoa(String.fromCharCode.apply(null, hashArray))
                    .replace(/\+/g, '-')
                    .replace(/\//g, '_')
                    .replace(/=+$/, '');
                console.log('Generated code challenge:', hashBase64);
                return hashBase64;
            }.bind(this));
    },

    // Base64URL encoding
    base64URLEncode: function(str) {
        return btoa(str)
            .replace(/\+/g, '-')
            .replace(/\//g, '_')
            .replace(/=/g, '');
    }
}); 