using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocationCleaned
{
    [ComVisible(true)]
    public partial class frmMap : Form
    {
        /// <summary>
        /// 经纬度坐标
        /// </summary>
        public Location Location { get; set; } = new Location();
        public frmMap()
        {
            InitializeComponent();
        }

        private void frmMap_Load(object sender, EventArgs e)
        {
            this.webBrowser1.ScriptErrorsSuppressed = true;
            var text = @"<html xmlns='http://www.w3.org/1999/xhtml\'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=utf-8' />
    <title>站点地图</title>
    <style type='text/css'>
        body, html, #allmap
        {
            width: 100%;
            height: 100%;
            overflow: hidden;
            margin: 0;
        }
        #l-map
        {
            height: 100%;
            width: 78%;
            float: left;
            border-right: 2px solid #bcbcbc;
        }
        #r-result
        {
            height: 100%;
            width: 20%;
            float: left;
        }
    </style>
    <script type='text/javascript' src='http://api.map.baidu.com/api?v=3.0&ak=GjHGmPzAPs0hH6KIcpvmoAzt'></script>
</head>
<body>
    <div id='allmap'>
    </div>
    <div style='float:right;position: absolute;
    top: 1px;
    left: 60px;background-color:;font-size: 12px;
    line-height: 23px;'>
        <div style='float:left'>请输入关键字搜索</div>
        <div id='r-result' style='float:left'><input type='text' id='suggestId' size='20' value=''  style='width:300px;' /></div>
        <div id='searchResultPanel' style='border:1px solid #C0C0C0;width:300px;height:auto; display:none;'></div>
    </div>
</body>
</html>
<script type='text/javascript'>
    document.oncontextmenu=new Function('event.returnValue=false;'); document.onselectstart=new Function('event.returnValue=false;'); 
    var marker;
    var map = new BMap.Map('allmap');               // 创建Map实例
    var point = new BMap.Point(114.05979, 22.545453);    // 创建点坐标(经度,纬度)
    map.centerAndZoom(point, 8);                   // 初始化地图,设置中心点坐标和地图大小级别
    //map.addOverlay(new BMap.Marker(point));         // 给该坐标加一个红点标记
    map.addControl(new BMap.NavigationControl());   // 添加平移缩放控件
    map.addControl(new BMap.ScaleControl());        // 添加比例尺控件
    map.addControl(new BMap.OverviewMapControl());  //添加缩略地图控件
    map.addControl(new BMap.MapTypeControl());      //添加地图类型控件
    map.enableScrollWheelZoom();                    //启用滚轮放大缩小
    var geoc = new BMap.Geocoder();  
    map.addEventListener('click', function (e) {
        checkMaker();
        point = new BMap.Point(e.point.lng, e.point.lat);
        marker = new BMap.Marker(point);
        map.addOverlay(marker);
        var pt = e.point;
        geoc.getLocation(pt, function (rs) {
            var addComp = rs.addressComponents;
            var address = [];
            if (addComp.province.length > 0) {
                address.push(addComp.province);
            }
            if (addComp.city.length > 0) {
                address.push(addComp.city);
            }
            if (addComp.district.length > 0) {
                address.push(addComp.district);
            }
            if (addComp.street.length > 0) {
                address.push(addComp.street);
            }
            if (addComp.streetNumber.length > 0) {
                address.push(addComp.streetNumber);
            }
            window.external.position(e.point.lat, e.point.lng, address.join(','));
        });
    });
    function G(id) {
        return document.getElementById(id);
    }
    var ac = new BMap.Autocomplete(    //建立一个自动完成的对象
       {
           'input': 'suggestId'
          , 'location': map
       });
    ac.addEventListener('onhighlight', function (e) {  //鼠标放在下拉列表上的事件
        var str = '';
        var _value = e.fromitem.value;
        var value = '';
        if (e.fromitem.index > -1) {
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str = 'FromItem<br />index = ' + e.fromitem.index + '<br />value = ' + value;
        value = '';
        if (e.toitem.index > -1) {
            _value = e.toitem.value;
            value = _value.province + _value.city + _value.district + _value.street + _value.business;
        }
        str += '<br />ToItem<br />index = ' + e.toitem.index + '<br />value = ' + value;
        G('searchResultPanel').innerHTML = str;
    });
    var myValue;
    ac.addEventListener('onconfirm', function (e) {    //鼠标点击下拉列表后的事件
        var _value = e.item.value;
        myValue = _value.province + _value.city + _value.district + _value.street + _value.business;
        G('searchResultPanel').innerHTML = 'onconfirm<br />index = ' + e.item.index + '<br />myValue = ' + myValue;
        setPlace();
    });
    function setPlace() {
        map.clearOverlays();    //清除地图上所有覆盖物
        function myFun() {
            var pp = local.getResults().getPoi(0).point;    //获取第一个智能搜索的结果
            checkMaker();
            map.centerAndZoom(pp, 15);
            marker=new BMap.Marker(pp);
            map.addOverlay(marker);    //添加标注
         var pt = pp;
            geoc.getLocation(pt, function (rs) {
                var addComp = rs.addressComponents;
                var address = [];
                if (addComp.province.length > 0) {
                    address.push(addComp.province);
                }
                if (addComp.city.length > 0) {
                    address.push(addComp.city);
                }
                if (addComp.district.length > 0) {
                    address.push(addComp.district);
                }
                if (addComp.street.length > 0) {
                    address.push(addComp.street);
                }
                if (addComp.streetNumber.length > 0) {
                    address.push(addComp.streetNumber);
                }
                window.external.position(pp.lat, pp.lng, address.join(','));
            });
        }
        var local = new BMap.LocalSearch(map, { //智能搜索
            onSearchComplete: myFun
        });
        local.search(myValue);
    }
    function checkMaker() {
        if (marker != null)
            map.removeOverlay(marker);
    };
    function evaluatepoint(log,lat){
        checkMaker();
        point = new BMap.Point(log,lat);
        map.centerAndZoom(point, 15);
        marker = new BMap.Marker(point);
        map.addOverlay(marker);
        var pt = point;
        geoc.getLocation(pt, function (rs) {
            var addComp = rs.addressComponents;
            var address = [];
            if (addComp.province.length > 0) {
                address.push(addComp.province);
            }
            if (addComp.city.length > 0) {
                address.push(addComp.city);
            }
            if (addComp.district.length > 0) {
                address.push(addComp.district);
            }
            if (addComp.street.length > 0) {
                address.push(addComp.street);
            }
            if (addComp.streetNumber.length > 0) {
                address.push(addComp.streetNumber);
            }
            window.external.position(pt.lat,pt.lng, address.join(','));
        });
    };</script>";
            this.webBrowser1.DocumentText = text;
            this.webBrowser1.ObjectForScripting = this;
        }
        public void position(string a_0, string a_1, string b_0)
        {
            this.label3.Text = (double.Parse( a_1) - 0.01169).ToString();
            this.label4.Text = (double.Parse(a_0) - 0.00293).ToString();
            this.label5.Text = b_0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Location.Longitude = double.Parse(label3.Text);
            Location.Latitude = double.Parse(label4.Text);
            Close();
        }
        public void Alert(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
