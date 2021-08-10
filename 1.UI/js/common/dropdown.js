$(document).ready(function () {
  $(".dropdown").click(function () {
    // slide dropdown
    $(this).find(".dropdown-data").slideToggle(400);
    //toggle icon
    var icon = $(this).find(".dropdown-icon");
    switch (icon.hasClass("down")) {
      case true:
        icon.removeClass("rotate");
        icon.removeClass("down");
        icon.addClass("up");
        break;
      case false:
        icon.addClass("rotate");
        icon.removeClass("up");
        icon.addClass("down");
        break;
      default:
        icon.addClass("rotate");
        icon.removeClass("up");
        icon.addClass("down");
        break;
    }

    //click outside close dropdown
    var dropDown = $(this);
    $(document).click(function (e) {
      if (!dropDown.is(e.target) && dropDown.has(e.target).length === 0) {
        var isOpened = dropDown.find(".dropdown-data").css("display");
        if (isOpened == "block") {
          dropDown.find(".dropdown-data").slideUp(400);
          icon.removeClass("rotate");
          icon.removeClass("down");
          icon.addClass("up");
        }
      }
    });

    // item selected
    $(".dropdown-item").click(function () {
      $(this)
        .parent()
        .find(".dropdown-item")
        .css({ "background-color": "#fff", color: "#000" });
      $(this)
        .parent()
        .find(".dropdown-item-icon")
        .css({ visibility: "hidden" });
      var dropDownText = $(this).parent().parent().find(".dropdown-text");
      var item = $(this).find(".dropdown-item-text");
      var valueItem = $(this).find(".dropdown-item-text").attr("value");
      dropDownText.text(item.text());
      dropDownText.attr("value", valueItem);
      $(this).css({ "background-color": "#019160", color: "#fff" });
      $(this).find(".dropdown-item-icon").css({ visibility: "visible" });
    });
  });

  function RotateUp() {}
});
