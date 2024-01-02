document.addEventListener("DOMContentLoaded", function () {
  // handle add class sticky when scroll over header
  var header = document.querySelector(".header");
  var headerHeight = header.offsetHeight + 34;
  window.addEventListener("scroll", function () {
    if (window.scrollY > headerHeight) {
      header.classList.add("sticky");
    } else {
      header.classList.remove("sticky");
    }
  });
});

function replaceUrlParam(url, paramName, paramValue) {
  if (paramValue == null) {
    paramValue = "";
  }
  var pattern = new RegExp("\\b(" + paramName + "=).*?(&|#|$)");
  if (url.search(pattern) >= 0) {
    return url.replace(pattern, "$1" + paramValue + "$2");
  }
  url = url.replace(/[?#]$/, "");
  return (
    url + (url.indexOf("?") > 0 ? "&" : "?") + paramName + "=" + paramValue
  );
}

function handleToogleFilter(id, buttonId) {
  var buttonDom = document.getElementById(buttonId);
  var filterId = document.getElementById(id);

  buttonDom && buttonDom.classList.toggle("--active");
  filterId && filterId.classList.toggle("--hidden");
}

function handleScrollToTop() {
  window.scrollTo({ top: 0, behavior: "smooth" });
}
