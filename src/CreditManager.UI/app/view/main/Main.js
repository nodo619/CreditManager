Ext.define('CreditManager.UI.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',
        'CreditManager.UI.service.AuthService',

        'CreditManager.UI.view.main.MainController',
        'CreditManager.UI.view.main.MainModel',
        'CreditManager.UI.view.main.List'
    ],

    controller: 'main',
    viewModel: 'main',

    ui: 'navigation',

    tabBarHeaderPosition: 1,
    titleRotation: 0,
    tabRotation: 0,

    header: {
        layout: {
            align: 'stretchmax'
        },
        title: {
            text: 'Credit Manager',
            flex: 0
        },
        iconCls: 'fa-th-list'
    },

    dockedItems: [{
        xtype: 'toolbar',
        dock: 'top',
        cls: 'main-toolbar',
        items: [{
            xtype: 'component',
            reference: 'userInfo',
            cls: 'user-info',
            html: 'Loading...',
            hidden: true
        }, {
            xtype: 'button',
            reference: 'loginLink',
            cls: 'login-link',
            text: 'Login',
            hidden: true,
            listeners: {
                click: 'onLoginClick'
            }
        }, {
            xtype: 'button',
            reference: 'logoutLink',
            cls: 'logout-link',
            text: 'Logout',
            hidden: true,
            listeners: {
                click: 'onLogoutClick'
            }
        }]
    }],

    tabBar: {
        flex: 1,
        layout: {
            align: 'stretch',
            overflowHandler: 'none'
        }
    },

    responsiveConfig: {
        tall: {
            headerPosition: 'top'
        },
        wide: {
            headerPosition: 'left'
        }
    },

    defaults: {
        bodyPadding: 20,
        tabConfig: {
            responsiveConfig: {
                wide: {
                    iconAlign: 'left',
                    textAlign: 'left'
                },
                tall: {
                    iconAlign: 'top',
                    textAlign: 'center',
                    width: 120
                }
            }
        }
    },

    items: [
        {
            title: 'Home',
            iconCls: 'x-fa fa-home',
            layout: 'fit',
            items: [{
                xtype: 'mainlist'
            }]
        },
        {
            title: 'Users',
            iconCls: 'x-fa fa-user',
            bind: {
                html: '{loremIpsum}'
            }
        },
        {
            title: 'Groups',
            iconCls: 'x-fa fa-users',
            bind: {
                html: '{loremIpsum}'
            }
        },
        {
            title: 'Settings',
            iconCls: 'x-fa fa-cog',
            bind: {
                html: '{loremIpsum}'
            }
        }
    ]
}); 