package kabam.rotmg.chat.control
{
import com.company.assembleegameclient.game.AGameSprite;
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.parameters.Parameters;
import kabam.rotmg.chat.model.ChatMessage;
import kabam.rotmg.game.signals.AddTextLineSignal;
import kabam.rotmg.text.model.TextKey;
import kabam.rotmg.ui.model.HUDModel;
import kabam.rotmg.stage3D.Renderer;

public class ParseChatMessageCommand
{
    [Inject]
    public var data:String;

    [Inject]
    public var hudModel:HUDModel;

    [Inject]
    public var addTextLine:AddTextLineSignal;

    public function ParseChatMessageCommand()
    {
        super();
    }

    public function execute() : void
    {
        var _loc1_:Array = null;
        var _loc2_:Number = NaN;
        if(this.data.charAt(0) == "/")
        {
            _loc1_ = this.data.substr(1,this.data.length).split(" ");
            switch(_loc1_[0])
            {
                case "help":
                    this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME,TextKey.HELP_COMMAND));
                return;
                case "mscale":
                    if(_loc1_.length > 1)
                    {
                        _loc2_ = Number(_loc1_[1]);
                        if(_loc2_ >= 0.5 && _loc2_ <= 2)
                        {
                            Parameters.data_.mscale = _loc2_;
                            Parameters.save();
                            this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME,"Map scale: " + (_loc2_)));
                        }
                        else
                        {
                            this.addTextLine.dispatch(ChatMessage.make(Parameters.SERVER_CHAT_NAME,"Map scale only accept values between 0.5 to 2.0"));
                        }
                    }
                    else
                    {
                        this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME,"Map scale: " + Parameters.data_.mscale));
                    }
                return;
                case "test":
                    this.addTextLine.dispatch(ChatMessage.make(Parameters.HELP_CHAT_NAME,"ranktext.x " + Parameters.data_.rankx));
                return;
            }
        }
        this.hudModel.gameSprite.gsc_.playerText(this.data);
    }
}
}
