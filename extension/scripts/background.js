// background.js
chrome.action.onClicked.addListener(function (tab) {
  chrome.windows.create({
    url: chrome.runtime.getURL("popup.html"),
    type: "popup",
    width: 800, // Điều chỉnh kích thước theo mong muốn
    height: 1000,
  });
});
