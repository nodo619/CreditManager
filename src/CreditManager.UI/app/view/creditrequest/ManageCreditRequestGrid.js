Ext.define('CreditManager.UI.view.creditrequest.ManageCreditRequestGrid', {
    extend: 'Ext.grid.Panel',
    xtype: 'managecreditrequestgrid',
    controller: 'managecreditrequest',
    title: 'Manage Credit Requests',
    requires: [
        'CreditManager.UI.store.ManageCreditRequestStore'
    ],

    store: {
        type: 'managecreditrequeststore'
    },

    columns: [
        {
            text: 'Customer',
            flex: 1,
            renderer: function (v, m, record) {
                const customer = record.get('customer') || {};
                return `${customer.firstName || ''} ${customer.lastName || ''}`.trim();
            }
        },
        {
            text: 'Credit Type',
            dataIndex: 'creditType',
            flex: 1,
            renderer: function (value) {
                return CreditManager.UI.util.EnumMapper.creditType[value] || 'Unknown';
            }
        },
        {
            text: 'Amount',
            dataIndex: 'amount',
            flex: 1
        },
        {
            text: 'Currency',
            dataIndex: 'currencyCode',
            flex: 1
        },
        {
            text: 'Period',
            flex: 1,
            renderer: function (v, m, record) {
                return `${record.get('periodYears')}y ${record.get('periodMonths')}m ${record.get('periodDays')}d`;
            }
        },
        {
            text: 'Status',
            dataIndex: 'status',
            flex: 1,
            renderer: function (value) {
                return CreditManager.UI.util.EnumMapper.status[value] || 'Unknown';
            }
        },
        {
            xtype: 'actioncolumn',
            text: 'Actions',
            width: 150,
            items: [
                {
                    iconCls: 'x-fa fa-check action-icon',
                    tooltip: 'Approve',
                    scope: 'controller',
                    handler: 'onApproveClick',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 2; // Only when Sent
                    }
                },
                {
                    iconCls: 'x-fa fa-times action-icon',
                    tooltip: 'Reject',
                    scope: 'controller',
                    handler: 'onRejectClick',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 2; // Only when Sent
                    }
                }
            ]
        }
    ],

    bbar: {
        xtype: 'pagingtoolbar',
        displayInfo: true
    }
});