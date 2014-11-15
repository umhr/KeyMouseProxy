package  
{
	/**
	 * ...
	 * @author umhr
	 */
	public class MousePoint 
	{
		public var type:String;
		public var x:Number;
		public var y:Number;
		static public const TYPE_POSITION:String = "xy";
		static public const TYPE_RIGHT_DOWN:String = "mr";
		static public const TYPE_LEFT_DOWN:String = "ml";
		static public const START_STR:String = String.fromCharCode(0xE000);//0x7B = "{";
		static public const SEPARATOR_STR:String = String.fromCharCode(0xE001);
		static public const END_STR:String = String.fromCharCode(0xE002);//0x7D = "}";
		public function MousePoint(type:String = "xy", x:Number = 0, y:Number = 0) 
		{
			this.type = type;
			this.x = x;
			this.y = y;
		}
		
		public function toString():String {
			
			var sx:String = x.toFixed(4);
			var sy:String = y.toFixed(4);
			return START_STR + type + SEPARATOR_STR + sx + SEPARATOR_STR + sy + END_STR;
		}
	}

}