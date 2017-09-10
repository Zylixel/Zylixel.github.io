package com.company.assembleegameclient.map
{
import com.company.assembleegameclient.objects.GameObject;
import com.company.assembleegameclient.parameters.Parameters;
import com.company.assembleegameclient.util.RandomUtil;
import flash.geom.Matrix3D;
import flash.geom.PerspectiveProjection;
import flash.geom.Rectangle;
import flash.geom.Vector3D;

public class Camera
{

    public static const lN_:Vector3D = new Vector3D(0,0,1);

    public static const CENTER_SCREEN_RECT:Rectangle = new Rectangle(-300,-325,600,600);

    public static const OFFSET_SCREEN_RECT:Rectangle = new Rectangle(-300,-450,600,600);

    private static const SCREENSHOT_SCREEN_RECT:Rectangle = new Rectangle(-400,-325,800,600);

    private static const SLIM_SCREENSHOT_SCREEN_RECT:Rectangle = new Rectangle(-400,-275,800,500);


    private const MAX_JITTER:Number = 0.5;

    private const JITTER_BUILDUP_MS:int = 10000;

    public var x_:Number;

    public var y_:Number;

    public var z_:Number;

    public var angleRad_:Number;

    public var clipRect_:Rectangle;

    public var pp_:PerspectiveProjection;

    public var maxDist_:Number;

    public var maxDistSq_:Number;

    public var isHallucinating_:Boolean = false;

    public var wToS_:Matrix3D;

    public var wToV_:Matrix3D;

    public var vToS_:Matrix3D;

    private var nonPPMatrix_:Matrix3D;

    private var p_:Vector3D;

    private var f_:Vector3D;

    private var u_:Vector3D;

    private var r_:Vector3D;

    private var isJittering_:Boolean = false;

    private var jitter_:Number = 0;

    private var rd_:Vector.<Number>;

    public function Camera()
    {
        this.pp_ = new PerspectiveProjection();
        this.wToS_ = new Matrix3D();
        this.wToV_ = new Matrix3D();
        this.vToS_ = new Matrix3D();
        this.nonPPMatrix_ = new Matrix3D();
        this.p_ = new Vector3D();
        this.f_ = new Vector3D();
        this.u_ = new Vector3D();
        this.r_ = new Vector3D();
        this.rd_ = new Vector.<Number>(16,true);
        super();
        this.pp_.focalLength = 3;
        this.pp_.fieldOfView = 48;
        this.nonPPMatrix_.appendScale(50,50,50);
        this.f_.x = 0;
        this.f_.y = 0;
        this.f_.z = -1;
    }

    public function configureCamera(param1:GameObject, param2:Boolean) : void
    {
        var _loc3_:Rectangle = !!Parameters.data_.centerOnPlayer?CENTER_SCREEN_RECT:OFFSET_SCREEN_RECT;
        var _loc4_:Rectangle = _loc3_.clone();
            _loc4_.x = -(Parameters.data_.mscale * 50 * (1 / 2));
            _loc4_.y = -(Parameters.data_.mscale * 50 * (_loc4_.y / -600));
            _loc4_.width = Parameters.data_.mscale * 50;
            _loc4_.height = Parameters.data_.mscale * 50;
        if(Parameters.screenShotMode_)
        {
            if(!Parameters.screenShotSlimMode_)
            {
                _loc4_ = SCREENSHOT_SCREEN_RECT;
            }
            else
            {
                _loc4_ = SLIM_SCREENSHOT_SCREEN_RECT;
            }
        }
        var _loc5_:Number = Parameters.data_.cameraAngle;
        this.configure(param1.x_,param1.y_,12,_loc5_,_loc4_);
        this.isHallucinating_ = param2;
    }

    public function startJitter() : void
    {
        this.isJittering_ = true;
        this.jitter_ = 0;
    }

    public function update(param1:Number) : void
    {
        if(this.isJittering_ && this.jitter_ < this.MAX_JITTER)
        {
            this.jitter_ = this.jitter_ + param1 * this.MAX_JITTER / this.JITTER_BUILDUP_MS;
            if(this.jitter_ > this.MAX_JITTER)
            {
                this.jitter_ = this.MAX_JITTER;
            }
        }
    }

    public function configure(param1:Number, param2:Number, param3:Number, param4:Number, param5:Rectangle) : void
    {
        if(this.isJittering_)
        {
            param1 = param1 + RandomUtil.plusMinus(this.jitter_);
            param2 = param2 + RandomUtil.plusMinus(this.jitter_);
        }
        this.x_ = param1;
        this.y_ = param2;
        this.z_ = param3;
        this.angleRad_ = param4;
        this.clipRect_ = param5;
        this.p_.x = param1;
        this.p_.y = param2;
        this.p_.z = param3;
        this.r_.x = Math.cos(this.angleRad_);
        this.r_.y = Math.sin(this.angleRad_);
        this.r_.z = 0;
        this.u_.x = Math.cos(this.angleRad_ + Math.PI / 2);
        this.u_.y = Math.sin(this.angleRad_ + Math.PI / 2);
        this.u_.z = 0;
        this.rd_[0] = this.r_.x;
        this.rd_[1] = this.u_.x;
        this.rd_[2] = this.f_.x;
        this.rd_[3] = 0;
        this.rd_[4] = this.r_.y;
        this.rd_[5] = this.u_.y;
        this.rd_[6] = this.f_.y;
        this.rd_[7] = 0;
        this.rd_[8] = this.r_.z;
        this.rd_[9] = -1;
        this.rd_[10] = this.f_.z;
        this.rd_[11] = 0;
        this.rd_[12] = -this.p_.dotProduct(this.r_);
        this.rd_[13] = -this.p_.dotProduct(this.u_);
        this.rd_[14] = -this.p_.dotProduct(this.f_);
        this.rd_[15] = 1;
        this.wToV_.rawData = this.rd_;
        this.vToS_ = this.nonPPMatrix_;
        this.wToS_.identity();
        this.wToS_.append(this.wToV_);
        this.wToS_.append(this.vToS_);
        var _loc6_:Number = this.clipRect_.width / (2 * 50);
        var _loc7_:Number = this.clipRect_.height / (2 * 50);
        this.maxDist_ = Math.sqrt(_loc6_ * _loc6_ + _loc7_ * _loc7_) + 1;
        this.maxDistSq_ = this.maxDist_ * this.maxDist_;
    }
}
}
