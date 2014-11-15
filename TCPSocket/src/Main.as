package 
{
	import flash.desktop.NativeApplication;
	import flash.events.Event;
	import flash.display.Sprite;
	import flash.display.StageAlign;
	import flash.display.StageScaleMode;
	import flash.ui.Multitouch;
	import flash.ui.MultitouchInputMode;
	
	/**
	 * ...
	 * @author umhr
	 */
	public class Main extends Sprite 
	{
		
		public function Main():void 
		{
			stage.scaleMode = StageScaleMode.NO_SCALE;
			stage.align = StageAlign.TOP_LEFT;
			stage.addEventListener(Event.DEACTIVATE, deactivate);
			
			// touch or gesture?
			Multitouch.inputMode = MultitouchInputMode.TOUCH_POINT;
			
			// entry point
			addChild(new Canvas());
			// new to AIR? please read *carefully* the readme.txt files!
			
			return;
			var str:String = "{xy,0.458974,0.256911}";
			var startIndex:int = str.indexOf("{");
			var endIndex:int = str.indexOf("}");
			if (startIndex > -1 && endIndex > -1) {
				var result:String = str.substr(startIndex + 1, endIndex - 1);
				if (str.length > endIndex+1) {
					str = str.substr(endIndex + 1);
				}else {
					str = "";
				}
			}
			trace(str);
		}
		
		private function deactivate(e:Event):void 
		{
			// make sure the app behaves well (or exits) when in background
			//NativeApplication.nativeApplication.exit();
		}
		
	}
	
}