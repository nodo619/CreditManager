Ext.define('CreditManager.UI.view.creditrequest.MyCreditRequestGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'mycreditrequestgrid',
    controller: 'mycreditrequest',
    title: 'My Credit Requests',
    requires: [
        'CreditManager.UI.store.CreditRequestStore'
    ],

    store: {
        type: 'creditrequeststore'
    },

    columns: [
        { text: 'Amount', dataIndex: 'amount', flex: 1 },
        { text: 'Currency', dataIndex: 'currencyCode', flex: 1 },
        { text: 'Period', renderer: function(v, m, r) {
            return `${r.get('periodYears')}y ${r.get('periodMonths')}m ${r.get('periodDays')}d`;
        }, flex: 1 },
        {
            xtype: 'actioncolumn',
            text: 'Actions',
            width: 120,
            items: [
                {
                    iconCls: 'x-fa fa-eye action-icon',
                    tooltip: 'View',
                    handler: 'onViewClick'
                },
                {
                    iconCls: 'x-fa fa-edit action-icon',
                    tooltip: 'Edit',
                    handler: 'onEditClick'
                },
                {
                    iconCls: 'x-fa fa-trash action-icon',
                    tooltip: 'Delete',
                    handler: 'onDeleteClick'
                }
            ]
        }
    ],

    bbar: {
        xtype: 'pagingtoolbar',
        displayInfo: true
    }
});