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
                    iconCls: 'x-fa fa-paper-plane action-icon',
                    tooltip: 'Send',
                    scope: 'controller',
                    handler: 'onSendClick',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 1;
                    }
                },
                {
                    iconCls: 'x-fa fa-edit action-icon',
                    tooltip: 'Edit',
                    scope: 'controller',
                    handler: 'onEditClick',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 1;
                    }
                },
                {
                    iconCls: 'x-fa fa-times-circle action-icon',
                    tooltip: 'Cancel',
                    scope: 'controller',
                    handler: 'onCancelClick',
                    isDisabled: function (view, rowIndex, colIndex, item, record) {
                        return record.get('status') !== 1;
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