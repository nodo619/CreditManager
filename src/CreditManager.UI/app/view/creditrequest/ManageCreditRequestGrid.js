Ext.define('CreditManager.UI.view.creditrequest.ManageCreditRequestGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'managecreditrequestgrid',
    title: 'Manage Credit Requests',

    requires: [
        'CreditManager.UI.store.ManageCreditRequestStore'
    ],

    store: {
        type: 'managecreditrequeststore'
    },

    columns: [
        { text: 'ID', dataIndex: 'id', flex: 1 },
        { text: 'CustomerId', dataIndex: 'customerId', flex: 1 },
        { text: 'Amount', dataIndex: 'amount', flex: 1 },
        { text: 'Currency', dataIndex: 'currencyCode', flex: 1 }
    ],

    bbar: {
        xtype: 'pagingtoolbar',
        displayInfo: true
    }
});