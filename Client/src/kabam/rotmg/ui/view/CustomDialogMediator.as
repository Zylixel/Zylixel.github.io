package kabam.rotmg.ui.view {
import com.company.assembleegameclient.ui.dialogs.CustomDialog;

import kabam.rotmg.account.core.Account;
import kabam.rotmg.account.core.services.GetOffersTask;
import kabam.rotmg.account.core.signals.OpenMoneyWindowSignal;
import kabam.rotmg.dialogs.control.CloseDialogsSignal;

import robotlegs.bender.bundles.mvcs.Mediator;

public class CustomDialogMediator extends Mediator {

    [Inject]
    public var account:Account;
    [Inject]
    public var getOffers:GetOffersTask;
    [Inject]
    public var view:CustomDialog;
    [Inject]
    public var closeDialogs:CloseDialogsSignal;


    override public function initialize():void {
        this.getOffers.start();
        this.view.cancel.add(this.onCancel);
    }

    override public function destroy():void {
        this.view.cancel.remove(this.onCancel);
    }

    public function onCancel():void {
        this.closeDialogs.dispatch();
    }

}
}//package kabam.rotmg.ui.view
