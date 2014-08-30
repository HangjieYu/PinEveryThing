﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="passenger.aspx.cs" Inherits="PinEverything.Web.passenger" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <script src="js/jquery.min.js"></script>
    <script src="js/passenger.js"></script>
    <link href="css/css.css" rel="stylesheet">
    <title>我是乘客</title>
</head>
<body>
    <div class="sidebarBg">
        <div class="sidebarLeft sidebarPlayWidth"></div>
        <div class="sidebarPlay">
            <a href="play.html">打车</a>
        </div>
        <div class="sidebarTitle">
            我是乘客
        </div>
        <div class="clear"></div>
    </div>
    <div class="passengerTab">
        <div class="passengerTabLeft active passengerTabClick" rel="passengerList">
            <span class="passengerTabLeftIcon"></span>计划
        <div class="passengerTabArrow"></div>
        </div>
        <div class="passengerTabRight passengerTabClick" rel="passengerMap">
            <span class="passengerTabRightIcon"></span>附近
        <div class="passengerTabArrow"></div>
        </div>
        <div class="clear"></div>
    </div>
    <div class="passengerList">
<%--        <ul>
            <li>
                <div class="passengerListLeft">
                    <div class="passengerListLeftHead">
                        <img src="images/1.jpg" alt="" /></div>
                    <div class="passengerListLeftName">Elen</div>
                </div>
                <div class="passengerListRight">
                    <a href="detail.html">
                        <span class="passengerListArrow"></span>
                        <span class="passengerListArrowInner"></span>
                        <div class="passengerListFromAddress">中山西路虹桥路</div>
                        <div class="passengerListToAddress">长宁龙之梦</div>
                        <div class="passengerListTime">9:10 至 18:00</div>
                        <div class="passengerListCar">特斯拉</div>
                        <div class="passengerListDate">8月28日 16:00</div>
                    </a>
                </div>
                <div class="clear"></div>
            </li>
            <li class="passengerListPosition">
                <div class="passengerListLeft">
                    <div class="passengerListLeftHead">
                        <img src="images/1.jpg" alt="" /></div>
                    <div class="passengerListLeftName">Elen</div>
                </div>
                <div class="passengerListRight">
                    <a href="detail.html">
                        <span class="passengerListArrow"></span>
                        <span class="passengerListArrowInner"></span>
                        <div class="passengerListFromAddress">中山西路虹桥路</div>
                        <div class="passengerListToAddress">长宁龙之梦</div>
                        <div class="passengerListTime">9:10 至 18:00</div>
                        <div class="passengerListCar">特斯拉</div>
                        <div class="passengerListDate">8月28日 16:00</div>
                    </a>
                </div>
                <div class="clear"></div>
            </li>
            <li>
                <div class="passengerListLeft">
                    <div class="passengerListLeftHead">
                        <img src="images/1.jpg" alt="" /></div>
                    <div class="passengerListLeftName">Elen</div>
                </div>
                <div class="passengerListRight">
                    <a href="detail.html">
                        <span class="passengerListArrow"></span>
                        <span class="passengerListArrowInner"></span>
                        <div class="passengerListFromAddress">中山西路虹桥路</div>
                        <div class="passengerListToAddress">长宁龙之梦</div>
                        <div class="passengerListTime">9:10 至 18:00</div>
                        <div class="passengerListCar">特斯拉</div>
                        <div class="passengerListDate">8月28日 16:00</div>
                    </a>
                </div>
                <div class="clear"></div>
            </li>
            <li class="passengerListPosition">
                <div class="passengerListLeft">
                    <div class="passengerListLeftHead">
                        <img src="images/1.jpg" alt="" /></div>
                    <div class="passengerListLeftName">Elen</div>
                </div>
                <div class="passengerListRight">
                    <a href="detail.html">
                        <span class="passengerListArrow"></span>
                        <span class="passengerListArrowInner"></span>
                        <div class="passengerListFromAddress">中山西路虹桥路</div>
                        <div class="passengerListToAddress">长宁龙之梦</div>
                        <div class="passengerListTime">9:10 至 18:00</div>
                        <div class="passengerListCar">特斯拉</div>
                        <div class="passengerListDate">8月28日 16:00</div>
                    </a>
                </div>
                <div class="clear"></div>
            </li>
            <li>
                <div class="passengerListLeft">
                    <div class="passengerListLeftHead">
                        <img src="images/1.jpg" alt="" /></div>
                    <div class="passengerListLeftName">Elen</div>
                </div>
                <div class="passengerListRight">
                    <a href="detail.html">
                        <span class="passengerListArrow"></span>
                        <span class="passengerListArrowInner"></span>
                        <div class="passengerListFromAddress">中山西路虹桥路</div>
                        <div class="passengerListToAddress">长宁龙之梦</div>
                        <div class="passengerListTime">9:10 至 18:00</div>
                        <div class="passengerListCar">特斯拉</div>
                        <div class="passengerListDate">8月28日 16:00</div>
                    </a>
                </div>
                <div class="clear"></div>
            </li>
            <li class="passengerListPosition">
                <div class="passengerListLeft">
                    <div class="passengerListLeftHead">
                        <img src="images/1.jpg" alt="" /></div>
                    <div class="passengerListLeftName">Elen</div>
                </div>
                <div class="passengerListRight">
                    <a href="detail.html">
                        <span class="passengerListArrow"></span>
                        <span class="passengerListArrowInner"></span>
                        <div class="passengerListFromAddress">中山西路虹桥路</div>
                        <div class="passengerListToAddress">长宁龙之梦</div>
                        <div class="passengerListTime">9:10 至 18:00</div>
                        <div class="passengerListCar">特斯拉</div>
                        <div class="passengerListDate">8月28日 16:00</div>
                    </a>
                </div>
                <div class="clear"></div>
            </li>
        </ul>--%>
    </div>
    <div class="passengerMap">
    </div>
</body>
</html>
<script>
    $(function () {
        $(".passengerTabClick").click(function () {
            $(this).addClass("active").siblings().removeClass("active");
            var className = $(this).attr("rel");
            if (className == 'passengerList') {
                $(".passengerList").fadeIn();
                $(".passengerMap").fadeOut();
            } else {
                $(".passengerList").fadeOut();
                $(".passengerMap").fadeIn();
            }
        });
    });
</script>
