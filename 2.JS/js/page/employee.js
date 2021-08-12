$(document).ready(function () {
  flag = 0;
  employeeID = null;
  employeeCode = null;
  //Load dữ liệu trên table
  loadData();
  // Load dữ liệu lên dropdown phòng ban
  loadDropdownDepartment();
  // Load dữ liệu lên dropdown vị trí
  loadDropdownPosition();
  // Phân trang
  amountPage();
  // mở form chi tiết thông tin nhân viên
  $("#btn-add").click(btnAddClick);

  // ẩn form chi tiết thông tin nhân viên
  $(".close").click(function () {
    $(".modal").hide();
  });

  // ẩn form chi tiết thông tin nhân viên
  $(".button-info-cancel").click(function () {
    $(".modal").hide();
  });

  // ẩn hiện nút delete trong form input
  $(".input-search").on("keyup", function () {
    var keyword = $(this).val();
    if (keyword == 0) {
      $(".icon-input-delete").css("display", "none");
    } else {
      $(".icon-input-delete").css("display", "block");
    }
  });
  $(".icon-input-delete").click(function () {
    $(".input-search").val("");
    $(".icon-input-delete").hide();
    loadData();
  });
  moneyInput($(".txtSalary"));
  // validate dữ liệu
  validateData();

  //reload dữ liệu trên table
  $(".m-btn-refresh").click(loadData);

  // lưu dữ liệu nhân viên
  $(".m-btn-save").click(btnSaveClick);

  // sửa thông tin nhân viên
  $("table").on("click", "tbody tr", function () {
    flag = 0;
    employeeID = $(this).attr("employeeID");
    editEmployee();
  });

  // xóa thông tin nhân viên
  $("table").on("mousedown", "tbody tr", function (event) {
    event.preventDefault();
    if (event.which == 3) {
      // lấy employeeID
      employeeID = $(this).attr("employeeID");
      employeeCode = $(this).attr("employeeCode");
      deleteEmployee();
    }
  });

  //filter input
  $(".input-search").on("input", function () {
    let inputVal = $(this).val();
    $.ajax({
      url: "http://cukcuk.manhnv.net/v1/Employees",
      method: "GET",
    }).done(function (res) {
      var data = res;
      $.each(data, function (index, item) {
        if (
          item.EmployeeCode.includes(inputVal) ||
          item.FullName.includes(inputVal) ||
          item.PhoneNumber.includes(inputVal)
        ) {
          var dob = item.DateOfBirth;
          var salary = item.Salary;
          salary = formatMoney(salary + "");
          dob = formatDate(dob);
          var row = $(
            `<tr>
                  <td title = ` +
              item.EmployeeCode +
              `>` +
              formatText(item.EmployeeCode) +
              `</td>
                  <td title = ` +
              item.FullName +
              `>` +
              formatText(item.FullName) +
              `</td>
                  <td title = ` +
              item.GenderName +
              `>` +
              formatText(item.GenderName) +
              `</td>
                  <td style="text-align: center;" title = ` +
              item.DateOfBirth +
              `>` +
              formatText(dob) +
              `</td>
                  <td title = ` +
              item.PhoneNumber +
              `>` +
              formatText(item.PhoneNumber) +
              `</td>
                  <td title = ` +
              item.Email +
              `>` +
              formatText(item.Email) +
              `</td>
                  <td title = ` +
              item.PositionName +
              `>` +
              formatText(item.PositionName) +
              `</td>
                  <td title = ` +
              item.DepartmentName +
              `>` +
              formatText(item.DepartmentName) +
              `</td>
                  <td style="text-align: right" >` +
              formatText(salary) +
              `</td>
                  <td  title = ` +
              item.WorkStatus +
              `>` +
              formatText(item.WorkStatus) +
              `</td>
                </tr>`
          );
          $("table tbody").empty();
          $("table tbody").html(row);
        }
      });
    }).fail(function(res)
    {

    });
    
  });
});


/**-------------------------------
 * Hàm sửa thông tin nhân viên
 * CreateBy: LQNhat(21/07/2021)
 */
function editEmployee() {
  try {
    $(".modal").show();
    $(".modal input[required]").removeClass("bg-focus");
    $(".modal input[required]").removeClass("bg-red");
    $(".modal input[required]").removeAttr("title");
    $(".modal input[required]").removeAttr("data-toggle");
    $(".modal input[required]").removeAttr("data-placement");
    $(".modal input[required]").removeAttr("data-original-title");
    // lấy employeeID
    $.ajax({
      url: `http://cukcuk.manhnv.net/v1/Employees/` + employeeID + ``,
      method: "GET",
    })
      .done(function (res) {
        //binding dữ liệu lên form
        $(".txtEmployeeCode").val(res.EmployeeCode);
        $(".txtFullName").val(res.FullName);
        $(".txtDateOfBirth").val(formatDateForm(res.DateOfBirth));
        $(".txtIdentityNumber").val(res.IdentityNumber);
        $(".txtIdentityDate").val(formatDateForm(res.IdentityDate));
        $(".txtIdentityPlace").val(res.IdentityPlace);
        $(".txtEmail").val(res.Email);
        $(".txtPhoneNumber").val(res.PhoneNumber);
        $(".txtPersonalTaxCode").val(res.PersonalTaxCode);
        $(".txtSalary").val(formatMoney(res.Salary + ""));
        $(".txtJoinDate").val(formatDateForm(res.JoinDate));
        autoSelectedDropdown("gender", res.Gender);
        autoSelectedDropdown("workstt", res.WorkStatus);
        autoSelectedDropdown("position", res.PositionId);
        autoSelectedDropdown("department", res.DepartmentId);
      })
      .fail(function (res) {
        alert("Có lỗi xảy ra, vui lòng liên hệ với MISA để được hỗ trợ");
      });
  } catch (error) {
    console.log(error);
  }
}

/**----------------------------
 * Hàm xóa thông tin nhân viên
 * CreateBy: LQNhat(21/07/2021)
 */
function deleteEmployee() {
  try {
    $(".modal-delete").show();
    $(".employeeCodeDelete").text(employeeCode);
    $(".close-delete").click(function () {
      $(".modal-delete").hide();
    });
    $(".m-btn-cancel-delete").click(function () {
      $(".modal-delete").hide();
    });
    $(".m-btn-confirm").click(function () {
      $.ajax({
        url: `http://cukcuk.manhnv.net/v1/Employees/` + employeeID + ``,
        method: "DELETE",
      })
        .done(function (res) {
          alert("Xóa nhân viên thành công");
          $(".modal-delete").hide();
          loadData();
        })
        .fail(function (res) {});
    });
  } catch (error) {
    console.log(error);
  }
}

/**-----------------------------------
 * Load dữ liệu vị trí lên dropdown
 * CreateBy: LQNhat(20/07/2021)
 */
function loadDropdownPosition() {
  try {
    $.ajax({
      url: "http://cukcuk.manhnv.net/v1/Positions",
      method: "GET",
    })
      .done(function (res) {
        var data = res;
        $.each(data, function (index, item) {
          var div = $(
            `<div class="dropdown-item" value = "` +
              item.PositionId +
              `">
                         <div class="dropdown-item-icon">
                           <i class="fas fa-check"></i>
                         </div>
                         <div class="dropdown-item-text" value = "` +
              item.PositionId +
              `">` +
              item.PositionName +
              `</div>
                       </div>`
          );
          $(".dropdown-position").append(div);
          $(".dropdown-position-info")
            .find(".dropdown-item")
            .first()
            .css({ "background-color": "#019160", color: "#fff" });
          $(".dropdown-position-info")
            .find(".dropdown-item-icon")
            .first()
            .css({ visibility: "visible" });
          $(".dropdown-position-info")
            .parent()
            .find(".dropdown-text")
            .text(
              $(".dropdown-position-info").find(".dropdown-item").first().text()
            );
          // autoSelectedDropdown("position", item.PositionId);
        });
      })
      .fail(function (res) {
        alert("Vui lòng liên hệ với MISA để được hỗ trợ");
      });
  } catch (error) {
    console.log(error);
  }
}

/**------------------------------------
 * Hàm đếm tổng số bản ghi trong table
 * CreateBy: LQNhat(23/07/2021)
 */
function amountPage() {
  try {
    $.ajax({
      url: "http://cukcuk.manhnv.net/v1/Employees",
      method: "GET",
    })
      .done(function (res) {
        var amount = res.length;
        $(".amountPage").text(amount);
      })
      .fail(function (res) {
        alert(" Đã có lỗi xảy ra, xin vui lòng liên hệ MISA để được hỗ trợ");
      });
  } catch (error) {
    console.log(error);
  }
}

/**------------------------------------
 * Load dữ liệu phòng ban lên dropdown
 * CreateBy: LQNhat(20/07/2021)
 */
function loadDropdownDepartment() {
  try {
    $.ajax({
      url: "http://cukcuk.manhnv.net/api/Department",
      method: "GET",
    })
      .done(function (res) {
        var data = res;
        $.each(data, function (index, item) {
          var itemDepart = $(
            `<div class="dropdown-item" value = "` +
              item.DepartmentId +
              `">
                         <div class="dropdown-item-icon">
                           <i class="fas fa-check"></i>
                         </div>
                         <div class="dropdown-item-text" value = "` +
              item.DepartmentId +
              `">` +
              item.DepartmentName +
              `</div>
                       </div>`
          );

          $(".dropdown-department").append(itemDepart);
          $(".dropdown-department-info")
            .find(".dropdown-item")
            .first()
            .css({ "background-color": "#019160", color: "#fff" });
          $(".dropdown-department-info")
            .find(".dropdown-item-icon")
            .first()
            .css({ visibility: "visible" });
          $(".dropdown-department-info")
            .parent()
            .find(".dropdown-text")
            .text(
              $(".dropdown-department-info")
                .find(".dropdown-item")
                .first()
                .text()
            );
        });
      })
      .fail(function (res) {
        alert("Vui lòng liên hệ với MISA để được hỗ trợ");
      });
  } catch (error) {
    console.log(error);
  }
}

/**---------------------------------------------------
 * Hàm auto seletec cho các dropdown khi bấm form sửa
 * @param {any} nameControl
 * @param {any} val
 * CreatBy:LQNhat(22/07/2021)
 */
function autoSelectedDropdown(nameControl, val) {
  try {
    const currentControl = $(`.${nameControl}`);
    let options = currentControl.find(".dropdown-item");
    let flag = false;
    for (let i = 0; i < options.length; i++) {
      var element = $(options[i]);
      if (element.attr("value") == val) {
        element
          .parent()
          .find(".dropdown-item")
          .css({ "background-color": "#fff", color: "#000" });
        element
          .parent()
          .find(".dropdown-item-icon")
          .css({ visibility: "hidden" });
        var dropDownText = element.parent().parent().find(".dropdown-text");
        var item = element.find(".dropdown-item-text");
        var valueItem = element.find(".dropdown-item-text").attr("value");
        dropDownText.text(item.text());
        dropDownText.attr("value", valueItem);
        element.css({ "background-color": "#019160", color: "#fff" });
        element.find(".dropdown-item-icon").css({ visibility: "visible" });
        flag = true;
      }
    }
    if (!flag) {
      $(options[0])
        .parent()
        .find(".dropdown-item")
        .css({ "background-color": "#fff", color: "#000" });
      $(options[0])
        .parent()
        .find(".dropdown-item-icon")
        .css({ visibility: "hidden" });
      var dropDownText = $(options[0]).parent().parent().find(".dropdown-text");
      var item = $(options[0]).find(".dropdown-item-text");
      var valueItem = $(options[0]).find(".dropdown-item-text").attr("value");
      dropDownText.text(item.text());
      dropDownText.attr("value", valueItem);
      $(options[0]).css({ "background-color": "#019160", color: "#fff" });
      $(options[0]).find(".dropdown-item-icon").css({ visibility: "visible" });
    }
  } catch (error) {
    console.log(error);
  }
}

/**-----------------------------------
 * Hàm bắt sự kiện nút thêm nhân viên
 * CreateBy: LQNhat(21/07/2021)
 */
function btnAddClick() {
  try {
    flag = 1;
    $(".modal").show();
    // reset form
    $(".modal input").val(null);
    $(".modal input[required]").removeClass("bg-focus");
    $(".modal input[required]").removeClass("bg-red");
    $(".modal input[required]").removeAttr("title");
    $(".modal input[required]").removeAttr("data-toggle");
    $(".modal input[required]").removeAttr("data-placement");
    $(".modal input[required]").removeAttr("data-original-title");
    autoSelectedDropdown("gender", 1);
    autoSelectedDropdown("workstt", 0);
    autoSelectedDropdown("position", "");
    autoSelectedDropdown("department", "");

    //gán mã nhân viên mới
    $.ajax({
      url: "http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode",
      method: "GET",
    })
      .done(function (res) {
        $(".txtEmployeeCode").val(res);
        $(".txtEmployeeCode").focus();
      })
      .fail(function (res) {
        alert("Vui lòng liên hệ với MISA để được hỗ trợ");
      });
  } catch (error) {
    console.log(error);
  }
}

/**-------------------------------------------------
 * Hàm bắt sự kiện cho nút lưu thông tin nhân viên
 * CreatBy: LQNhat(21/07/2021)
 */
function btnSaveClick() {
  try {
    // validate toàn bộ dữ liệu

    validateData();
    // thu thập dữ liệu build thành object nhân viên
    let employee = {};
    employee.EmployeeCode = $(".txtEmployeeCode").val();
    employee.FullName = $(".txtFullName").val();
    employee.DateOfBirth = $(".txtDateOfBirth").val();
    employee.IdentityNumber = $(".txtIdentityNumber").val();
    employee.IdentityDate = $(".txtIdentityDate").val();
    employee.IdentityPlace = $(".txtIdentityPlace").val();
    employee.Email = $(".txtEmail").val();
    employee.PhoneNumber = $(".txtPhoneNumber").val();
    employee.PersonalTaxCode = $(".txtPersonalTaxCode").val();
    employee.Salary = Number($(".txtSalary").val().replaceAll(".", ""));
    employee.JoinDate = $(".txtJoinDate").val();
    employee.Gender = $(".txtGender").attr("value");
    employee.DepartmentId = $(".departmentId").attr("value");
    employee.PositionId = $(".positionId").attr("value");
    employee.WorkStatus = $(".workStatus").attr("value");
    let method = "POST";
    let url = "http://cukcuk.manhnv.net/v1/Employees";
    if (flag == 0) {
      method = "PUT";
      url = `http://cukcuk.manhnv.net/v1/Employees/` + employeeID + ``;
    }
    // gọi đến API lưu dữ liệu
    if (
      $(".txtEmail").val() == "" ||
      $(".txtEmployeeCode").val() == "" ||
      $(".txtIdentityNumber").val() == "" ||
      $(".txtFullName").val() == "" ||
      $(".txtPhoneNumber").val() == ""
    ) {
      alert("Vui lòng nhập đủ các trường thông tin cần thiết");
    } else {
      $.ajax({
        url: url,
        method: method,
        data: JSON.stringify(employee),
        dataType: "json",
        contentType: "application/json",
        // async:
      })
        .done(function (res) {
          if (flag == 1) {
            alert("Thêm mới thành công");
          } else {
            alert("Sửa thành công");
          }
          //Load lại dữ liệu trên table
          loadData();
          //hide form chi tiết thông tin
          $(".modal").hide();
        })
        .fail(function (res) {
          alert("Vui lòng liên hệ với MISA để được hỗ trợ");
        });
    }
  } catch (error) {
    console.log(error);
  }
}

/**-------------------------------
 * Load dữ liệu lên table
 * CreateBy: LQNhat(19/07/2021)
 */
function loadData() {
  try {
    // clean dữ liệu cũ đã có trong table
    $("tbody").empty();
    amountPage();
    // đọc API lấy dữ liệu về
    $.ajax({
      url: "http://cukcuk.manhnv.net/v1/Employees",
      method: "GET",
    })
      .done(function (res) {
        var data = res;
        $.each(data, function (index, item) {
          var dob = item.DateOfBirth;
          var salary = item.Salary;
          salary = formatMoney(salary + "");
          dob = formatDate(dob);
          var trHTML = $(
            `<tr>
                  <td title = ` +
              item.EmployeeCode +
              `>` +
              formatText(item.EmployeeCode) +
              `</td>
                  <td title = ` +
              item.FullName +
              `>` +
              formatText(item.FullName) +
              `</td>
                  <td title = ` +
              item.GenderName +
              `>` +
              formatText(item.GenderName) +
              `</td>
                  <td style="text-align: center;" title = ` +
              item.DateOfBirth +
              `>` +
              formatText(dob) +
              `</td>
                  <td title = ` +
              item.PhoneNumber +
              `>` +
              formatText(item.PhoneNumber) +
              `</td>
                  <td title = ` +
              item.Email +
              `>` +
              formatText(item.Email) +
              `</td>
                  <td title = ` +
              item.PositionName +
              `>` +
              formatText(item.PositionName) +
              `</td>
                  <td title = ` +
              item.DepartmentName +
              `>` +
              formatText(item.DepartmentName) +
              `</td>
                  <td style="text-align: right" >` +
              formatText(salary) +
              `</td>
                  <td  title = ` +
              item.WorkStatus +
              `>` +
              formatText(item.WorkStatus) +
              `</td>
                </tr>`
          );
          $(trHTML).attr("employeeID", item.EmployeeId);
          $(trHTML).attr("employeeCode", item.EmployeeCode);
          $("table tbody").append(trHTML);
        });
      })
      .fail(function (res) {
        alert("Vui lòng liên hệ với MISA để được hỗ trợ");
      });
  } catch (error) {
    console.log(error);
  }
}

/**-------------------------------
 * Hàm định dạng lại tiền lương
 * @param {Number} money
 * @returns Number
 * CreateBy: LQNhat(23/07/2021)
 */
function formatMoneyForm(money) {
  try {
    money += "";
    if (money != null) {
      money.replaceAll(".", "");
      let onlynumber = "";
      for (var i = 0; i < money.length; i++) {
        if (!isNaN(parseInt(money[i], 10))) {
          onlynumber += money[i];
        }
      }
      return Number(onlynumber).toLocaleString("vi");
    }
    return 0;
  } catch (error) {
    console.log(error);
  }
}

/**--------------------------------
 * Hàm format tiền lương
 * @param {Number} money
 * CreateBy: LQNhat(23/07/2021)
 */
function moneyInput(money) {
  try {
    money.on("input", function () {
      let myinput = money.val();
      $(this).css("caret-color", "black");
      if (!isNaN(parseFloat(myinput))) {
        $(this).css("caret-color", "red");
      }
      res = formatMoneyForm(myinput);
      money.val(res);
    });
  } catch (error) {
    console.log(error);
  }
}

/**--------------------------------------------------------
 * Hàm format lại định dạng ngày tháng năm để đẩy lên form
 * @param {date} date
 * @returns định dạng năm - tháng - ngày
 * CreateBy: LQNhat(23/07/2021)
 */
function formatDateForm(date) {
  try {
    let dateString = "";
    if (date) {
      let newDate = new Date(date);
      var day = newDate.getDate(),
        month = newDate.getMonth() + 1,
        year = newDate.getFullYear();
      day = day < 10 ? "0" + day : day;
      month = month < 10 ? "0" + month : month;
      dateString = year + "-" + month + "-" + day;
    } else {
      dateString = "";
    }
    return dateString;
  } catch (error) {
    console.log(error);
  }
}

/**----------------------------------------------
 * Format Dữ liệu ngày tháng sang ngày/tháng/năm
 * @param {any} date  bất cứ kiểu dữ liệu gì
 * @returns  day/month/year dạng String
 * CreateBy: LQNhat (19/07/2021)
 */
function formatDate(date) {
  try {
    let dateString = "";
    if (date) {
      let newDate = new Date(date);
      var day = newDate.getDate(),
        month = newDate.getMonth() + 1,
        year = newDate.getFullYear();
      day = day < 10 ? "0" + day : day;
      month = month < 10 ? "0" + month : month;
      dateString = month + "/" + day + "/" + year;
    } else {
      dateString = "";
    }
    return dateString;
  } catch (error) {
    console.log(error);
  }
}

/**-----------------------------------
 * Hàm định dạng hiển thị tiền lương
 * @param {String} money Số tiền lương
 * @returns  số tiền lương đã format dạng String
 * CreateBy: LQNhat(19/07/2021)
 */
var formatMoney = function (par) {
  try {
    let pattern = "";
    let result = "";
    let counter = 0;
    for (let i = par.length - 1; i >= 0; i--) {
      pattern = par[i] + pattern;
      if (pattern.length == 3) {
        if (counter != 0) result = pattern + "." + result;
        else result = pattern;
        pattern = "";
        counter++;
      }
    }
    if (pattern.length != 0) result = pattern + "." + result;
    return result;
  } catch (error) {
    console.log(error);
  }
};

/**----------------------------------------------
 * Hàm định dạng dữ liệu text
 * @param {any} par bất cứ kiểu dữ liệu gì
 * @returns dữ liệu dạng String hoặc dữ liệu rỗng
 * CreateBy: LQNhat(20/7/2021)
 */
function formatText(par) {
  try {
    return typeof par != "undefined" &&
      par != null &&
      par != "null" &&
      par != "n.ull"
      ? par.toString()
      : "";
  } catch (error) {
    console.log(error);
  }
}

/**----------------------------
 * Hàm định dạng Email
 * @param {any} email
 * @returns true or false
 * CreateBy: LQNhat(21/07/2021)
 */
function formatEmail(email) {
  try {
    var regex =
      /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!regex.test(email)) {
      return false;
    } else {
      return true;
    }
  } catch (error) {
    console.log(error);
  }
}

/**---------------------------------------------
 * Hàm format chứng minh thư nhân dân/căn cước
 * @param {any} number
 * @returns true or false
 * CreateBy: LQNhat(21/07/2021)
 */
function formatNumber(number) {
  try {
    var regex = /^[0-9]+$/;
    if (!regex.test(number)) {
      return false;
    } else {
      return true;
    }
  } catch (error) {
    console.log(error);
  }
}

/**--------------------------------
 * Hàm format họ và tên nhân viên
 * @param {any} fullName
 * @returns true or false
 * CreatBy: LQNhat(21/07/2021)
 */
function formatFullName(fullName) {
  try {
    var regex =
      /[^a-z0-9A-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]/;
    if (!regex.test(fullName)) {
      return false;
    } else {
      return true;
    }
  } catch (error) {
    console.log(error);
  }
}

function formatNumberPhone(numberPhone) {
  try {
    var regex = /^([0-9])|(\([0-9]{3}\)\s+[0-9]{3}\-[0-9]{4})/;
    if (!regex.test(numberPhone)) {
      return false;
    } else {
      return true;
    }
  } catch (error) {
    console.log(error);
  }
}

/**----------------------------
 * hàm validate dữ liệu
 * CreateBy: LQNhat(21/07/2021)
 */
function validateData() {
  try {
    // Các trường bắt buộc nhập: mã nhân viên, họ và tên, cmnd, email, SĐT
    $("input[required]").blur(function () {
      // kiểm tra thông tin có nhập hay không
      let value = $(this).val();
      if (value == "") {
        $(this).addClass("bg-red");
        $(this).attr("title", "Thông tin này bắt buộc phải nhập");
        $(this).attr("data-toggle", "tooltip");
        $(this).attr("data-placement", "bottom");
        $(this).tooltip();
        $(this).focus(function () {
          $(this).addClass("bg-focus");
        });
        $(this).removeClass("bg-focus");
      } else {
        $(this).removeClass("bg-red");
        $(this).removeAttr("title");
        $(this).removeAttr("data-toggle");
        $(this).removeAttr("data-placement");
        $(this).focus(function () {
          $(this).addClass("bg-focus");
        });
        $(this).removeClass("bg-focus");
        $(this).removeAttr("title");
        $(this).removeAttr("data-toggle");
        $(this).removeAttr("data-placement");
        $(this).removeAttr("data-original-title");
      }
    });

    //validate email
    $(".txtEmail").change(function () {
      let valEmail = $(".txtEmail").val();
      if (formatEmail(valEmail) == false) {
        alert("Email sai định dạng, vui lòng nhập lại ");
      }
    });

    //valdate mã nhân viên
    $(".txtEmployeeCode").change(function () {
      $.ajax({
        url: "http://cukcuk.manhnv.net/v1/Employees",
        method: "GET",
      }).done(function (res) {
        var data = res;
        $.each(data, function (index, item) {
          if ($(".txtEmployeeCode").val() == item.EmployeeCode) {
            alert("Trùng mã nhân viên, vui lòng nhập lại");
            $(".txtEmployeeCode").focus();
          }
        });
      });
    });

    // validate CMND
    $(".txtIdentityNumber").change(function () {
      let valNumber = $(".txtIdentityNumber").val();
      if (formatNumber(valNumber) == false) {
        alert("CMND sai định dạng, vui lòng nhập lại ");
      }
    });

    //validate fullname
    $(".txtFullName").change(function () {
      let valFullName = $(".txtFullName").val();
      if (formatFullName(valFullName) == false) {
        alert("Họ và tên sai định dạng, vui lòng nhập lại ");
      }
    });

    //validate numberPhone
    $(".txtPhoneNumber").change(function () {
      let valNumberPhone = $(".txtPhoneNumber").val();
      if (formatNumberPhone(valNumberPhone) == false) {
        alert("Số điện thoại sai định dạng, vui lòng nhập lại ");
      }
    });
  } catch (error) {
    console.log(error);
  }
}
