Ext.define('CreditManager.UI.view.main.MainController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.main',

    requires: [
        'CreditManager.UI.service.JwtHelper'
    ],

    init: function() {
        var me = this;
        var vm = this.getViewModel();

        const token = localStorage.getItem('access_token');

        if (token) {
            const userData = CreditManager.UI.service.JwtHelper.parseJwt(token);
            console.log('Decoded user data:', userData);

            vm.set('currentUser', {
                displayName: `${userData.given_name} ${userData.family_name}`,
                role: parseInt(userData.role)
            });

            var displayName = userData.given_name && userData.family_name 
                ? `${userData.given_name} ${userData.family_name}` 
                : userData.name || userData.sub;

            // Set entire currentUser object to trigger binding
            vm.set('currentUser', {
                username: userData.nameid || userData.sub,
                displayName: displayName,
                role: userData.role || 'User'
            });
        }
    },

    onLogoutClick: function() {
        localStorage.removeItem('access_token');
        window.location.reload();
    },
    
    onMyCreditsClick: function() {
        var me = this;
        var content = me.lookup('contentArea');
    
        content.removeAll();
        content.add({
            xtype: 'mycreditrequestgrid'
        });
    },
    
    onManageCreditsClick: function() {
        var me = this;
        var content = me.lookup('contentArea');
    
        content.removeAll();
        content.add({
            xtype: 'managecreditrequestgrid'
        });
    }
});