package  
{
	
	import flash.display.Sprite;
	import flash.events.Event;
	import flash.events.MouseEvent;
	import flash.events.TouchEvent;
	import flash.geom.Point;
	import flash.geom.Rectangle;
	/**
	 * ...
	 * @author umhr
	 */
	public class Touchpad extends Sprite 
	{
		private var _padArea:Rectangle;
		
		public function Touchpad() 
		{
			init();
		}
		private function init():void 
		{
			if (stage) onInit();
			else addEventListener(Event.ADDED_TO_STAGE, onInit);
		}

		private function onInit(event:Event = null):void 
		{
			removeEventListener(Event.ADDED_TO_STAGE, onInit);
			// entry point
			
			var w:int = stage.fullScreenWidth - 32;
			var h:int = w * (9 / 16);
			
			var touchArea:Sprite = new Sprite();
			touchArea.graphics.lineStyle(1);
			touchArea.graphics.beginFill(0xCCFFCC);
			touchArea.graphics.drawRect(0, 0, w, h);
			touchArea.graphics.endFill();
			touchArea.addEventListener(MouseEvent.MOUSE_MOVE, mouseMove);
			//touchArea.addEventListener(MouseEvent.CLICK, click);
			//touchArea.addEventListener(TouchEvent.TOUCH_TAP, click);
			//touchArea.addEventListener(TouchEvent.TOUCH_MOVE, mouseMove);
			addChild(touchArea);
			
			var leftButton:Sprite = new Sprite();
			leftButton.graphics.lineStyle(1);
			leftButton.graphics.beginFill(0xCCCCFF);
			leftButton.graphics.drawRect(0, h+16, w*0.4, w*0.2);
			leftButton.graphics.endFill();
			leftButton.addEventListener(MouseEvent.MOUSE_DOWN, leftButton_mouseDown);
			addChild(leftButton);
			
			var rightButton:Sprite = new Sprite();
			rightButton.graphics.lineStyle(1);
			rightButton.graphics.beginFill(0xFFCCCC);
			rightButton.graphics.drawRect(w*0.6, h+16, w*0.4, w*0.2);
			rightButton.graphics.endFill();
			rightButton.addEventListener(MouseEvent.MOUSE_DOWN, rightButton_mouseDown);
			addChild(rightButton);
			
			x = 16;
			y = 200;
			
			_padArea = new Rectangle(x, y, touchArea.width, touchArea.height);
		}
		
		private function click(e:Event):void 
		{
			//trace(e.type);
			//trace("left button");
			setMousePoint(MousePoint.TYPE_LEFT_DOWN);
		}
		
		private function rightButton_mouseDown(e:MouseEvent):void 
		{
			//trace("right button");
			setMousePoint(MousePoint.TYPE_RIGHT_DOWN);
		}
		
		private function leftButton_mouseDown(e:MouseEvent):void 
		{
			//trace("left button");
			setMousePoint(MousePoint.TYPE_LEFT_DOWN);
		}
		
		private function mouseMove(e:Event):void 
		{
			setMousePoint(MousePoint.TYPE_POSITION);
		}
		
		private function setMousePoint(type:String):void {
			var point:MousePoint = new MousePoint(type);
			point.x = (stage.mouseX - x) / _padArea.width;
			point.y = (stage.mouseY - y) / _padArea.height;
			//trace(point);
			SocketManager.getInstance().sendRequest(point.toString());
		}
	}
	
}