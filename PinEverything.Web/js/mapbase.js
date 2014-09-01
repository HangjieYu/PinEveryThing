﻿
//封装StringBuilder
function StringBuilder() {
    this._string_ = new Array();
}
StringBuilder.prototype.Append = function (str) {
    this._string_.push(str);
}
StringBuilder.prototype.toString = function () {
    return this._string_.join("");
}
StringBuilder.prototype.AppendFormat = function () {
    if (arguments.length > 1) {
        var TString = arguments[0];
        if (arguments[1] instanceof Array) {
            for (var i = 0, iLen = arguments[1].length; i < iLen; i++) {
                var jIndex = i;
                var re = eval("/\\{" + jIndex + "\\}/g;");
                TString = TString.replace(re, arguments[1][i]);
            }
        } else {
            for (var i = 1, iLen = arguments.length; i < iLen; i++) {
                var jIndex = i - 1;
                var re = eval("/\\{" + jIndex + "\\}/g;");
                TString = TString.replace(re, arguments[i]);
            }
        }
        this.Append(TString);
    } else if (arguments.length == 1) {
        this.Append(arguments[0]);
    }
};


var mapObj, markers = [], cluster;

$(function () {
    initialize();
});

function initialize() {
    var
        lng = 121.23259,
        lat = 31.109207;
    var position = new AMap.LngLat(lng, lat);
    mapObj = new AMap.Map("container", {
        view: new AMap.View2D({//创建地图二维视口
            dragEnable: false,
            //center: position,//创建中心点坐标
            zoom: 14, //设置地图缩放级别
            rotation: 0 //设置地图旋转角度
        }),
        lang: "zh_cn"//设置地图语言类型，默认：中文简体
    });//创建地图实例

    mapObj.plugin(["AMap.ToolBar", "AMap.OverView", "AMap.Scale"], function () {
        //加载工具条
        tool = new AMap.ToolBar({
            direction: false,//隐藏方向导航
            ruler: true,//隐藏视野级别控制尺
            autoPosition: false//禁止自动定位
        });
        mapObj.addControl(tool);

        //加载鹰眼
        view = new AMap.OverView();
        mapObj.addControl(view);

        //加载比例尺
        scale = new AMap.Scale();
        mapObj.addControl(scale);
    });

    setTimeout(function () {
        getCurrLocation();
    }, 1000);

    $('.btnShowBannerMenu').on('click', function () {
        if ($(this).hasClass('rightHover')) {
            $(this).removeClass('rightHover');
            $('.warpBannerMenu').slideUp('fast');
        } else {
            $(this).addClass('rightHover');
            $('.warpBannerMenu').slideDown('fast');
        }
    });
}

//创建标注
function CreateMark(lat, lng) {
    var marker = new AMap.Marker({ //自定义构造AMap.Marker对象                 
        map: mapObj,
        position: new AMap.LngLat(lng, lat),
        offset: new AMap.Pixel(-10, -34),
        icon: "http://webapi.amap.com/images/0.png"
    });

    return marker;
}

//创建信息窗体
function CreateInfoWindow(marker, content) {
    var inforWindow = new AMap.InfoWindow({
        isCustom: false,
        offset: new AMap.Pixel(0, -23),
        content: content
    });
    AMap.event.addListener(marker, "click", function (e) {
        inforWindow.open(mapObj, marker.getPosition());
    });

    return inforWindow;
}

//创建圆
function CreateCircle(lat, lng, radius) {
    var circle = new AMap.Circle({
        map: mapObj,//要显示覆盖物的地图对象                 
        center: new AMap.LngLat(lng, lat),//圆心，基点                 
        radius: radius,//半径                 
        strokeColor: "#F33",//线颜色 
        strokeOpacity: 1,//线透明度                 
        strokeWeight: 3,//线宽                 
        fillColor: "#ee2200",//填充颜色                 
        fillOpacity: 0.35//填充透明度                 
    });

    return circle;
}

function PanTo(lat, lng) {
    mapObj.panTo(new AMap.LngLat(lng, lat));
}

function getCurrLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showCurrPosition, handleLocationError);
    }
    else {
        alert('浏览器不支持定位');
    }
}

function handleLocationError(error) {
    try {
        switch (error.code) {

            case 0:

                alert("获取位置信息出错！");

                break;

            case 1:

                alert("您设置了阻止该页面获取位置信息！");

                break;

            case 2:

                alert("浏览器无法确定您的位置！");
                break;

            case 3:
                alert("获取位置信息超时！");
                break;
        }
    } catch (e) {
        alert(e);
    }
}

function showCurrPosition(position) {
    var lat = position.coords.latitude, lng = position.coords.longitude;
    $.ajax({
        type: 'post',
        url: '/ajaxpage/user.aspx',
        data: {op:'UpdateLbs', lat: lat, lng: lng },
        success: function (result) {

            var m = CreateMark(result.lat, result.lng);
            var c = CreateCircle(result.lat, result.lng, 1000);
            PanTo(lat, lng);

            //弹出标注是我的位置
            var infoW = CreateInfoWindow(m, '<div class="winfoTitle">当前您在这里</div><div id="geocoderResult"></div>');
            infoW.open(mapObj, m.getPosition());

            geocoder(m.getPosition());
        }
    });
}

function geocoder(position) {
    var MGeocoder;
    //加载地理编码插件
    mapObj.plugin(["AMap.Geocoder"], function () {
        MGeocoder = new AMap.Geocoder({
            radius: 1000,
            extensions: "all"
        });
        //返回地理编码结果
        AMap.event.addListener(MGeocoder, "complete", geocoder_CallBack);
        //逆地理编码
        MGeocoder.getAddress(position);
    });
    //加点
    var marker = new AMap.Marker({
        map: mapObj,
        icon: new AMap.Icon({
            image: "http://api.amap.com/Public/images/js/mark.png",
            size: new AMap.Size(58, 30),
            imageOffset: new AMap.Pixel(-32, -0)
        }),
        position: position,
        offset: new AMap.Pixel(-5, -30)
    });
    mapObj.setFitView();
}

//回调函数
function geocoder_CallBack(data) {
    var roadinfo = new StringBuilder();
    //返回地址描述
    address = data.regeocode.formattedAddress;
    //返回周边道路信息
    roadinfo.Append("<div>");
    roadinfo.Append("<div class='roadInfoTitle'>周边道路信息</div>");
    for (var i = 0; i < data.regeocode.roads.length; i++) {
        //var color = (i % 2 === 0 ? '#fff' : '#eee');
        //roadinfo += "<tr style='background-color:" + color + "; margin:0; padding:0;'><td>道路：" + data.regeocode.roads[i].name + "</td><td>方向：" + data.regeocode.roads[i].direction + "</td><td>距离：" + data.regeocode.roads[i].distance + "米</td></tr>";
        roadinfo.AppendFormat("<div class='roadInfo'>道路：{0}&nbsp;&nbsp;方向：{1}&nbsp;&nbsp;距离：{2}</div>",
                data.regeocode.roads[i].name,
                data.regeocode.roads[i].direction,
                data.regeocode.roads[i].distance
            );
    }
    roadinfo.Append("</div>");
    document.getElementById("geocoderResult").innerHTML = roadinfo.toString();
}

//function showCurrPosition(position) {
//    var lat = position.coords.latitude, lng = position.coords.longitude;
//    var m = CreateMark(lat, lng);
//    var c = CreateCircle(lat, lng, 1000);
//    PanTo(lat, lng);

//    //弹出标注是我的位置
//    var infoW = CreateInfoWindow(m, '当前位置<br />附近有<span style="color:red;">100</span>条发布信息');
//    infoW.open(mapObj, m.getPosition());

//}