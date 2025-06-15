Ext.define('CreditManager.UI.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox'
    ],

    controller: 'main',
    viewModel: 'main',

    tabBarPosition: 0,

    defaults: {
        tab: {
            iconAlign: 'top'
        },
        styleHtmlContent: true
    },

    tabBar: {
        flex: 1,
        layout: {
            align: 'center',
            pack: 'center'
        }
    },

    items: [{
        title: 'Home',
        iconCls: 'x-fa fa-home',
        items: [{
            xtype: 'mainlist'
        }]
    }]
}); 