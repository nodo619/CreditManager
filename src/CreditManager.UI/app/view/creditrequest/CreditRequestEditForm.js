Ext.define('CreditManager.UI.view.creditrequest.CreditRequestEditForm', {
    extend: 'Ext.window.Window',
    xtype: 'creditrequesteditform',
    title: 'Edit Credit Request',
    modal: true,
    width: 400,
    controller: 'mycreditrequest',

    items: [
        {
            xtype: 'form',
            reference: 'editForm',
            bodyPadding: 10,
            defaults: {
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'numberfield',
                    name: 'amount',
                    fieldLabel: 'Amount',
                    allowBlank: false,
                    minValue: 0
                },
                {
                    xtype: 'textfield',
                    name: 'currencyCode',
                    fieldLabel: 'Currency',
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'comments',
                    fieldLabel: 'Comments'
                },
                {
                    xtype: 'combobox',
                    name: 'creditType',
                    fieldLabel: 'Credit Type',
                    store: CreditManager.UI.util.EnumMapper.getComboStore(CreditManager.UI.util.EnumMapper.creditType),
                    queryMode: 'local',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'periodYears',
                    fieldLabel: 'Period (Years)',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'periodMonths',
                    fieldLabel: 'Period (Months)',
                    minValue: 0
                },
                {
                    xtype: 'numberfield',
                    name: 'periodDays',
                    fieldLabel: 'Period (Days)',
                    minValue: 0
                }
            ]
        }
    ],

    buttons: [
        {
            text: 'Save',
            handler: 'onSaveEdit'
        },
        {
            text: 'Cancel',
            handler: function (btn) {
                btn.up('window').close();
            }
        }
    ]
});