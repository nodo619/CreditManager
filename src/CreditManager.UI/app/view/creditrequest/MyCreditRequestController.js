Ext.define('CreditManager.UI.view.creditrequest.MyCreditRequestController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.mycreditrequest',

    onSendClick: function (grid, rowIndex) {
        const record = grid.getStore().getAt(rowIndex);
        Ext.Msg.confirm('Confirm Send', 'Are you sure you want to send this credit request?', async function (choice) {
            if (choice === 'yes') {
                try {
                    await CreditManager.UI.service.ApiService.post(
                        `CreditRequests/${record.get('id')}/Send`
                    );
                    grid.getStore().load();
                } catch (err) {
                    Ext.Msg.alert('Error', err.message);
                }
            }
        });
    },

    onCancelClick: function (grid, rowIndex) {
        const record = grid.getStore().getAt(rowIndex);
        Ext.Msg.confirm('Confirm Cancel', 'Are you sure you want to cancel this credit request?', async function (choice) {
            if (choice === 'yes') {
                try {
                    await CreditManager.UI.service.ApiService.post(
                        `CreditRequests/${record.get('id')}/Cancel`
                    );
                    grid.getStore().load();
                } catch (err) {
                    Ext.Msg.alert('Error', err.message);
                }
            }
        });
    },

    onEditClick: function (grid, rowIndex) {
        const record = grid.getStore().getAt(rowIndex);

        const editWindow = Ext.create('CreditManager.UI.view.creditrequest.CreditRequestEditForm', {
            record: record
        });

        const form = editWindow.lookupReference('editForm').getForm();

        // Load existing data into form
        form.setValues({
            amount: record.get('amount'),
            currencyCode: record.get('currencyCode'),
            comments: record.get('comments'),
            creditType: record.get('creditType'),
            periodYears: record.get('periodYears'),
            periodMonths: record.get('periodMonths'),
            periodDays: record.get('periodDays')
        });

        editWindow.show();
    },

    onSaveEdit: async function (btn) {
        const win = btn.up('window');
        const form = win.lookupReference('editForm').getForm();

        if (!form.isValid()) {
            return;
        }

        const values = form.getValues();
        const record = win.record;
        const id = record.get('id');

        try {
            await CreditManager.UI.service.ApiService.put(`CreditRequests/${id}`, values);
            win.close();
            record.store.load();
        } catch (err) {
            Ext.Msg.alert('Error', err.message);
        }
    }
});