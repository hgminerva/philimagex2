// get parameters in url
function getUrlParameter(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null) {
        return "";
    } else {
        return results[1];
    }
}

function getUserFacilityData() {
    $.ajax({
        url: '/api/user/getByFacilityId/' + getUrlParameter("facilityId"),
        cache: false,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (userFacilityResult) {
            document.getElementById("defaultFacility").innerHTML = userFacilityResult.UserName;
        }
    });
}
