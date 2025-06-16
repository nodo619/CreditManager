Ext.define('CreditManager.UI.view.creditrequest.ManageCreditRequestController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.managecreditrequest',

    onApproveClick: function (grid, rowIndex) {
        const record = grid.getStore().getAt(rowIndex);
        Ext.Msg.confirm('Confirm Approve', 'Are you sure you want to approve this request?', async function (choice) {
            if (choice === 'yes') {
                try {
                    await CreditManager.UI.service.ApiService.post(
                        `CreditRequests/${record.get('id')}/Approve`
                    );
                    grid.getStore().load();
                } catch (err) {
                    Ext.Msg.alert('Error', err.message);
                }
            }
        });
    },

    onRejectClick: function (grid, rowIndex) {
        const record = grid.getStore().getAt(rowIndex);
        Ext.Msg.confirm('Confirm Reject', 'Are you sure you want to reject this request?', async function (choice) {
            if (choice === 'yes') {
                try {
                    await CreditManager.UI.service.ApiService.post(
                        `CreditRequests/${record.get('id')}/Reject`
                    );
                    grid.getStore().load();
                } catch (err) {
                    Ext.Msg.alert('Error', err.message);
                }
            }
        });
    }
});