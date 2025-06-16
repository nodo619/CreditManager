Ext.define('CreditManager.UI.service.ApiService', {
    singleton: true,

    identityUrl: 'https://localhost:7055',
    apiUrl: 'https://localhost:7107/api',


    login: async function (username, password) {
        const response = await fetch(`${this.identityUrl}/Account/Login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        if (!response.ok) {
            throw new Error('Invalid username or password');
        }

        const result = await response.json();

        // Use accessToken instead of token
        localStorage.setItem('access_token', result.accessToken);
    },

    register: async function (data) {
        const response = await fetch(`${this.identityUrl}/Account/Register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error('Registration failed');
        }
    },

    getAuthHeaders: function () {
        const token = localStorage.getItem('access_token');
        if (token) {
            return { 'Authorization': `Bearer ${token}` };
        }
        return {};
    },

    async get(endpoint) {
        const response = await fetch(`${this.apiUrl}/${endpoint}`, {
            headers: {
                'Content-Type': 'application/json',
                ...this.getAuthHeaders()
            }
        });

        if (!response.ok) {
            throw new Error('API request failed');
        }

        return await response.json();
    },

    async post(endpoint, data = null) {
        const response = await fetch(`${this.apiUrl}/${endpoint}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                ...this.getAuthHeaders()
            },
            body: data ? JSON.stringify(data) : null
        });

        if (!response.ok) {
            throw new Error('API request failed');
        }

        // Check if response has content
        const text = await response.text();
        if (!text) {
            return null; // no body
        }

        return JSON.parse(text);
    },

    async put(endpoint, data) {
        const response = await fetch(`${this.apiUrl}/${endpoint}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                ...this.getAuthHeaders()
            },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            throw new Error('API request failed');
        }

        return await response.text(); // In case backend returns empty 200 OK
    }
});