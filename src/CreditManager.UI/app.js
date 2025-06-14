/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'CreditManager.UI.Application',

    name: 'CreditManager.UI',

    requires: [
        // This will automatically load all classes in the CreditManager.UI namespace
        // so that application classes do not need to require each other.
        'CreditManager.UI.*'
    ],

    // The name of the initial view to create.
    mainView: 'CreditManager.UI.view.main.Main'
});
