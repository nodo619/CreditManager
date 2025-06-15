/**
 * This class is the view model for the Main view of the application.
 */
Ext.define('CreditManager.UI.view.main.MainModel', {
    extend: 'Ext.app.ViewModel',

    alias: 'viewmodel.main',

    data: {
        name: 'CreditManager.UI',

        loremIpsum: ' aliqua. Ut enimollit anim id est laborum.',
    
        currentUser: {
            displayName: '',
            role: 0
        }
    }

    //TODO - add data, formulas and/or methods to support your view
});
