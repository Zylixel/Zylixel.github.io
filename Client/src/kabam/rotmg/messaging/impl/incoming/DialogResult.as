package kabam.rotmg.messaging.impl.incoming {
import flash.utils.IDataInput;

public class DialogResult extends IncomingMessage {

    public var title_:String;
    public var desc_:String;

    public function DialogResult(_arg_1:uint, _arg_2:Function) {
        super(_arg_1, _arg_2);
    }

    override public function parseFromInput(_arg_1:IDataInput):void {
        this.title_ = _arg_1.readUTF();
        this.desc_ = _arg_1.readUTF();
    }

    override public function toString():String {
        return (formatToString("DIALOG", "title_", "desc_"));
    }


}
}//package kabam.rotmg.messaging.impl.incoming