Ext.define('CreditManager.UI.view.main.Main', {
    extend: 'Ext.panel.Panel',
    xtype: 'app-main',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',
        'CreditManager.UI.service.AuthService',

        'CreditManager.UI.view.main.MainController',
        'CreditManager.UI.view.main.MainModel'
    ],

    controller: 'main',
    viewModel: 'main',

    layout: 'fit',
    bodyPadding: 20,

    header: {
        layout: {
            align: 'stretchmax'
        },
        title: {
            text: 'Credit Manager',
            flex: 0
        }
    },

    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        items: [
            {
                xtype: 'component',
                reference: 'userInfo',
                cls: 'user-info',
                bind: { html: '{currentUser.displayName}' }
            },
            {
                xtype: 'button',
                text: 'My Credit Requests',
                bind: { hidden: '{currentUser.role === 0}' },
                handler: 'onMyCreditsClick'
            },
            {
                xtype: 'button',
                text: 'Manage Credit Requests',
                bind: { hidden: '{currentUser.role !== "2"}' },
                handler: 'onManageCreditsClick'
            },
            {
                xtype: 'button',
                reference: 'logoutLink',
                cls: 'logout-link',
                text: 'Logout',
                listeners: { click: 'onLogoutClick' }
            }
        ]
    }],
    items: [
        {
            xtype: 'panel',
            reference: 'contentArea',
            region: 'center',
            layout: 'card',
            items: []
        }
    ]
}); 