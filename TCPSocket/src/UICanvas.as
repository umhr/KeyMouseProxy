package  
{
	import com.bit101.components.Label;
	import com.bit101.components.PushButton;
	import com.bit101.components.Style;
	import com.bit101.components.Text;
	import com.bit101.components.TextArea;
	import flash.display.Sprite;
	import flash.events.Event;
	/**
	 * ...
	 * @author umhr
	 */
	public class UICanvas extends Sprite
	{
		private static var _instance:UICanvas;
		public function UICanvas(blocker:Blocker){init();}
		public static function getInstance():UICanvas{
			if ( _instance == null ) {_instance = new UICanvas(new Blocker());};
			return _instance;
		}
		
		private var _addressText:Text;
		private var _logArea:TextArea;
		private var _inputText:Text;
		private var _isEcho:Boolean;
		private var _sendButton:PushButton;
		private var _shield:Sprite = new Sprite();
		private var _text:String = "";
		private var _touchPad:Touchpad;
		
		private function init():void
		{
			setUI();
		}
		
		private function setUI():void 
		{
			Style.embedFonts = false;
			Style.fontName = "PF Ronda Seven";
			Style.fontSize = 36;
			
			_inputText = new Text(this, 580, 8);
			_inputText.width = 200;
			_inputText.height = 40;
			_inputText.addEventListener(Event.CHANGE, inputText_change);
			
			_sendButton = new PushButton(this, 800, 8, "BackSpace", onSend);
			_sendButton.width = 200;
			_sendButton.height = 60;
			
			//_logArea = new TextArea(this, 300, 8, "init");
			_logArea = new TextArea(this, 1100, 8, "init");
			_logArea.width = 400;
			_logArea.height = 130+53;
			
			// bind前はクリックさせないように
			_shield.graphics.beginFill(0xFFFFFF, 0.9);
			_shield.graphics.drawRect(0, 0, 1576, 200);
			_shield.graphics.endFill();
			addChild(_shield);
			
			new Label(this, 8, 8, "Address:Port");
			_addressText = new Text(this, 8, 66);
			_addressText.width = 350;
			_addressText.height = 50;
			_addressText.text = "192.168.0.6:50057";
			
			new PushButton(this, 370, 66, "Bind", onBind).height = 60;
			
			_touchPad = new Touchpad();
			addChild(_touchPad);
		}
		
		private function inputText_change(e:Event):void 
		{
			//onSend(null);
			//getInputText()
			var str:String = getInputText();
			if (str == "") {
				return;
			}
			SocketManager.getInstance().sendRequest(MousePoint.START_STR + "key" + MousePoint.SEPARATOR_STR + str + MousePoint.END_STR);
		}
		
		/**
		 * 127.0.0.1
		 */
		public function get address():String {
			return _addressText.text.split(":")[0];
		}
		/**
		 * 8087
		 */
		public function get port():int {
			return int(_addressText.text.split(":")[1]);
		}
		
		/**
		 * Echoボタンの表示をするかを設定します。
		 * @param	enabled
		 */
		public function setEchoButton(enabled:Boolean = false):void {
			var echoButton:PushButton = new PushButton(this, 360, 210, "set Echo", onEchoToggle);
			addChildAt(echoButton, 0);
			echoButton.toggle = true;
			echoButton.selected = !enabled;
			isEcho = !enabled;
		}
		
		private function onBind(event:Event):void 
		{
			PushButton(event.target).enabled = false;
			dispatchEvent(new Event("bind"));
			removeChild(_shield);
			_addressText.enabled = false;
		}
		
		private function onEchoToggle(event:Event):void 
		{
			isEcho = (event.target as PushButton).selected;
		}
		
		/**
		 * echoが有効か否かを返します。
		 */
		public function get isEcho():Boolean 
		{
			return _isEcho;
		}
		public function set isEcho(value:Boolean):void 
		{
			_inputText.enabled = !value;
			_sendButton.enabled = !value;
			_isEcho = value;
		}
		
		private function onSend(event:Event):void 
		{
			//dispatchEvent(new Event("send"));
			SocketManager.getInstance().sendRequest(MousePoint.START_STR + "key" + MousePoint.SEPARATOR_STR + String.fromCharCode(0x08) + MousePoint.END_STR);
		}
		
		/**
		 * メッセージ表示エリアにメッセージを追加します。
		 * @param	message
		 */
		public function addMessage(message:String):void {
			
			var text:String = message +"\n" + _logArea.text;
			text = text.substr(0, 1000);
			_logArea.text = text;
		}
		
		/**
		 * 入力エリアのテキストを返します。
		 * 返す際にテキストは削除されます。
		 * @return
		 */
		public function getInputText():String {
			
			
			var temp:String = _inputText.text;
			var text:String = temp.substr(_text.length);
			//_inputText.text = "";
			_text = temp;
			return text;
		}
	}
	
}
class Blocker { };