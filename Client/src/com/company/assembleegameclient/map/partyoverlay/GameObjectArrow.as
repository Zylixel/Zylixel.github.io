package com.company.assembleegameclient.map.partyoverlay
{
import com.company.assembleegameclient.map.Camera;
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.ui.menu.Menu;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import com.company.util.RectangleUtil;
import com.company.util.Trig;
import flash.display.DisplayObjectContainer;
import flash.display.Graphics;
import flash.display.Shape;
import flash.display.Sprite;
import flash.events.MouseEvent;
import flash.filters.DropShadowFilter;
import flash.geom.Point;
import flash.geom.Rectangle;

public class GameObjectArrow extends Sprite
{

    public static const SMALL_SIZE:int = 8;

    public static const BIG_SIZE:int = 11;

    public static const DIST:int = 3;

    private static var menu_:Menu = null;


    public var menuLayer:DisplayObjectContainer;

    public var lineColor_:uint;

    public var fillColor_:uint;

    public var go_:GameObject = null;

    public var extraGOs_:Vector.<GameObject>;

    public var mouseOver_:Boolean = false;

    private var big_:Boolean;

    private var arrow_:Shape;

    protected var tooltip_:ToolTip = null;

    private var tempPoint:Point;

    public function GameObjectArrow(param1:uint, param2:uint, param3:Boolean)
    {
        this.extraGOs_ = new Vector.<GameObject>();
        this.arrow_ = new Shape();
        this.tempPoint = new Point();
        super();
        this.lineColor_ = param1;
        this.fillColor_ = param2;
        this.big_ = param3;
        addChild(this.arrow_);
        this.drawArrow();
        addEventListener(MouseEvent.MOUSE_OVER,this.onMouseOver);
        addEventListener(MouseEvent.MOUSE_OUT,this.onMouseOut);
        addEventListener(MouseEvent.MOUSE_DOWN,this.onMouseDown);
        filters = [new DropShadowFilter(0,0,0,1,8,8)];
        visible = false;
    }

    public static function removeMenu() : void
    {
        if(menu_ != null)
        {
            if(menu_.parent != null)
            {
                menu_.parent.removeChild(menu_);
            }
            menu_ = null;
        }
    }

    protected function onMouseOver(param1:MouseEvent) : void
    {
        this.mouseOver_ = true;
        this.drawArrow();
    }

    protected function onMouseOut(param1:MouseEvent) : void
    {
        this.mouseOver_ = false;
        this.drawArrow();
    }

    protected function onMouseDown(param1:MouseEvent) : void
    {
        param1.stopImmediatePropagation();
    }

    protected function setToolTip(param1:ToolTip) : void
    {
        this.removeTooltip();
        this.tooltip_ = param1;
        if(this.tooltip_ != null)
        {
            addChild(this.tooltip_);
            this.positionTooltip(this.tooltip_);
        }
    }

    protected function removeTooltip() : void
    {
        if(this.tooltip_ != null)
        {
            if(this.tooltip_.parent != null)
            {
                this.tooltip_.parent.removeChild(this.tooltip_);
            }
            this.tooltip_ = null;
        }
    }

    protected function setMenu(param1:Menu) : void
    {
        this.removeTooltip();
        menu_ = param1;
        this.menuLayer.addChild(menu_);
    }

    public function setGameObject(param1:GameObject) : void
    {
        if(this.go_ != param1)
        {
            this.go_ = param1;
        }
        this.extraGOs_.length = 0;
        if(this.go_ == null)
        {
            visible = false;
        }
    }

    public function addGameObject(param1:GameObject) : void
    {
        this.extraGOs_.push(param1);
    }

    public function draw(param1:int, param2:Camera) : void
    {
        var _loc3_:Rectangle = null;
        var _loc4_:Number = NaN;
        var _loc5_:Number = NaN;
        if(this.go_ == null)
        {
            visible = false;
            return;
        }
        this.go_.computeSortVal(param2);
        _loc3_ = param2.clipRect_;
        _loc3_.x = 150 - (param2.clipRect_.width / 1.265); //* (param2.clipRect_.width / 1920);
        _loc3_.y = -60 - (param2.clipRect_.height / 1.265); //* (param2.clipRect_.height / 1080);
        _loc3_.width = (param2.clipRect_.width / 0.76) - 50; //* (param2.clipRect_.width / 1920);
        _loc3_.height = (param2.clipRect_.height / 0.70) + 100; //* (param2.clipRect_.height / 1080);
        if (Parameters.data_.scaleUI) {
            this.scaleY = stage.stageHeight / 600;
            this.scaleX = stage.stageWidth / 800;
        } else {
            this.scaleX = this.scaleY = 1;
        }
        _loc4_ = this.go_.posS_[0];
        _loc5_ = this.go_.posS_[1];
        if(!RectangleUtil.lineSegmentIntersectXY(_loc3_,0,0,_loc4_,_loc5_,this.tempPoint))
        {
            this.go_ = null;
            visible = false;
            return;
        }
        x = this.tempPoint.x;
        y = this.tempPoint.y;
        var _loc6_:Number = Trig.boundTo180(270 - Trig.toDegrees * Math.atan2(_loc4_,_loc5_));
        if(this.tempPoint.x < _loc3_.left + 5)
        {
            if(_loc6_ > 45)
            {
                _loc6_ = 45;
            }
            if(_loc6_ < -45)
            {
                _loc6_ = -45;
            }
        }
        else if(this.tempPoint.x > _loc3_.right - 5)
        {
            if(_loc6_ > 0)
            {
                if(_loc6_ < 135)
                {
                    _loc6_ = 135;
                }
            }
            else if(_loc6_ > -135)
            {
                _loc6_ = -135;
            }
        }
        if(this.tempPoint.y < _loc3_.top + 5)
        {
            if(_loc6_ < 45)
            {
                _loc6_ = 45;
            }
            if(_loc6_ > 135)
            {
                _loc6_ = 135;
            }
        }
        else if(this.tempPoint.y > _loc3_.bottom - 5)
        {
            if(_loc6_ > -45)
            {
                _loc6_ = -45;
            }
            if(_loc6_ < -135)
            {
                _loc6_ = -135;
            }
        }
        this.arrow_.rotation = _loc6_;
        if(this.tooltip_ != null)
        {
            this.positionTooltip(this.tooltip_);
        }
        visible = true;
    }

    private function positionTooltip(param1:ToolTip) : void
    {
        var _loc5_:Number = NaN;
        var _loc8_:Number = NaN;
        var _loc9_:Number = NaN;
        var _loc2_:Number = this.arrow_.rotation;
        var _loc3_:int = DIST + BIG_SIZE + 12;
        var _loc4_:Number = _loc3_ * Math.cos(_loc2_ * Trig.toRadians);
        _loc5_ = _loc3_ * Math.sin(_loc2_ * Trig.toRadians);
        var _loc6_:Number = param1.contentWidth_;
        var _loc7_:Number = param1.contentHeight_;
        if(_loc2_ >= 45 && _loc2_ <= 135)
        {
            _loc8_ = _loc4_ + _loc6_ / Math.tan(_loc2_ * Trig.toRadians);
            param1.x = (_loc4_ + _loc8_) / 2 - _loc6_ / 2;
            param1.y = _loc5_;
        }
        else if(_loc2_ <= -45 && _loc2_ >= -135)
        {
            _loc8_ = _loc4_ - _loc6_ / Math.tan(_loc2_ * Trig.toRadians);
            param1.x = (_loc4_ + _loc8_) / 2 - _loc6_ / 2;
            param1.y = _loc5_ - _loc7_;
        }
        else if(_loc2_ < 45 && _loc2_ > -45)
        {
            param1.x = _loc4_;
            _loc9_ = _loc5_ + _loc7_ * Math.tan(_loc2_ * Trig.toRadians);
            param1.y = (_loc5_ + _loc9_) / 2 - _loc7_ / 2;
        }
        else
        {
            param1.x = _loc4_ - _loc6_;
            _loc9_ = _loc5_ - _loc7_ * Math.tan(_loc2_ * Trig.toRadians);
            param1.y = (_loc5_ + _loc9_) / 2 - _loc7_ / 2;
        }
    }

    private function drawArrow() : void
    {
        var _loc1_:Graphics = this.arrow_.graphics;
        _loc1_.clear();
        var _loc2_:int = this.big_ || this.mouseOver_?int(BIG_SIZE):int(SMALL_SIZE);
        _loc1_.lineStyle(1,this.lineColor_);
        _loc1_.beginFill(this.fillColor_);
        _loc1_.moveTo(DIST,0);
        _loc1_.lineTo(_loc2_ + DIST,_loc2_);
        _loc1_.lineTo(_loc2_ + DIST,-_loc2_);
        _loc1_.lineTo(DIST,0);
        _loc1_.endFill();
        _loc1_.lineStyle();
    }
}
}
