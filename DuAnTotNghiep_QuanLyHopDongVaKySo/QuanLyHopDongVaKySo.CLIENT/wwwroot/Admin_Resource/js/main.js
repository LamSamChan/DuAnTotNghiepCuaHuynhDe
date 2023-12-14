/* global Chart, coreui */

/**
 * --------------------------------------------------------------------------
 * CoreUI Boostrap Admin Template (v4.2.2): main.js
 * Licensed under MIT (https://coreui.io/license)
 * --------------------------------------------------------------------------
 */

// Disable the on-canvas tooltip
Chart.defaults.pointHitDetectionRadius = 1;
Chart.defaults.plugins.tooltip.enabled = false;
Chart.defaults.plugins.tooltip.mode = "index";
Chart.defaults.plugins.tooltip.position = "nearest";
Chart.defaults.plugins.tooltip.external = coreui.ChartJS.customTooltips;
Chart.defaults.defaultFontColor = "#646470";
const random = (min, max) =>
  // eslint-disable-next-line no-mixed-operators
  Math.floor(Math.random() * (max - min + 1) + min);

// eslint-disable-next-line no-unused-vars
const cardChart1 = new Chart(document.getElementById("card-chart1"), {
  type: "line",
  data: {
    labels: ["January", "February", "March", "April", "May", "June", "July"],
    datasets: [
      {
        label: "My First dataset",
        backgroundColor: "transparent",
        borderColor: "rgba(255,255,255,.55)",
        pointBackgroundColor: coreui.Utils.getStyle("--cui-primary"),
        data: [65, 59, 84, 84, 51, 55, 40],
      },
    ],
  },
  options: {
    plugins: {
      legend: {
        display: false,
      },
    },
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false,
          drawBorder: false,
        },
        ticks: {
          display: false,
        },
      },
      y: {
        min: 30,
        max: 89,
        display: false,
        grid: {
          display: false,
        },
        ticks: {
          display: false,
        },
      },
    },
    elements: {
      line: {
        borderWidth: 1,
        tension: 0.4,
      },
      point: {
        radius: 4,
        hitRadius: 10,
        hoverRadius: 4,
      },
    },
  },
});

// eslint-disable-next-line no-unused-vars
const cardChart2 = new Chart(document.getElementById("card-chart2"), {
  type: "line",
  data: {
    labels: ["January", "February", "March", "April", "May", "June", "July"],
    datasets: [
      {
        label: "My First dataset",
        backgroundColor: "transparent",
        borderColor: "rgba(255,255,255,.55)",
        pointBackgroundColor: coreui.Utils.getStyle("--cui-info"),
        data: [1, 18, 9, 17, 34, 22, 11],
      },
    ],
  },
  options: {
    plugins: {
      legend: {
        display: false,
      },
    },
    maintainAspectRatio: false,
    scales: {
      x: {
        grid: {
          display: false,
          drawBorder: false,
        },
        ticks: {
          display: false,
        },
      },
      y: {
        min: -9,
        max: 39,
        display: false,
        grid: {
          display: false,
        },
        ticks: {
          display: false,
        },
      },
    },
    elements: {
      line: {
        borderWidth: 1,
      },
      point: {
        radius: 4,
        hitRadius: 10,
        hoverRadius: 4,
      },
    },
  },
});

// eslint-disable-next-line no-unused-vars
const cardChart3 = new Chart(document.getElementById("card-chart3"), {
  type: "line",
  data: {
    labels: ["January", "February", "March", "April", "May", "June", "July"],
    datasets: [
      {
        label: "My First dataset",
        backgroundColor: "rgba(255,255,255,.2)",
        borderColor: "rgba(255,255,255,.55)",
        data: [78, 81, 80, 45, 34, 12, 40],
        fill: true,
      },
    ],
  },
  options: {
    plugins: {
      legend: {
        display: false,
      },
    },
    maintainAspectRatio: false,
    scales: {
      x: {
        display: false,
      },
      y: {
        display: false,
      },
    },
    elements: {
      line: {
        borderWidth: 2,
        tension: 0.4,
      },
      point: {
        radius: 0,
        hitRadius: 10,
        hoverRadius: 4,
      },
    },
  },
});

// eslint-disable-next-line no-unused-vars
const cardChart4 = new Chart(document.getElementById("card-chart4"), {
  type: "bar",
  data: {
    labels: [
      "January",
      "February",
      "March",
      "April",
      "May",
      "June",
      "July",
      "August",
      "September",
      "October",
      "November",
      "December",
      "January",
      "February",
      "March",
      "April",
    ],
    datasets: [
      {
        label: "My First dataset",
        backgroundColor: "rgba(255,255,255,.2)",
        borderColor: "rgba(255,255,255,.55)",
        data: [78, 81, 80, 45, 34, 12, 40, 85, 65, 23, 12, 98, 34, 84, 67, 82],
        barPercentage: 0.6,
      },
    ],
  },
  options: {
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false,
      },
    },
    scales: {
      x: {
        grid: {
          display: false,
          drawTicks: false,
        },
        ticks: {
          display: false,
        },
      },
      y: {
        grid: {
          display: false,
          drawBorder: false,
          drawTicks: false,
        },
        ticks: {
          display: false,
        },
      },
    },
  },
});

//client chart
const option1 = document.getElementById("option1");
const option2 = document.getElementById("option2");
const option3 = document.getElementById("option3");
const datePickerContainer = document.getElementById("datePickerContainer");
const weekPickerContainer = document.getElementById("weekPickerContainer");
const yearPickerContainer = document.getElementById("yearPickerContainer");

option1.addEventListener("change", function () {
  datePickerContainer.style.display = option1.checked ? "block" : "none";
  weekPickerContainer.style.display = "none";
  yearPickerContainer.style.display = "none";
});

option2.addEventListener("change", function () {
  datePickerContainer.style.display = "none";
  weekPickerContainer.style.display = option2.checked ? "block" : "none";
  yearPickerContainer.style.display = "none";
});

option3.addEventListener("change", function () {
  datePickerContainer.style.display = "none";
  weekPickerContainer.style.display = "none";
  yearPickerContainer.style.display = option3.checked ? "block" : "none";
});

// Lấy JWT từ session
//const jwtToken = sessionStorage.getItem('token');

// Thiết lập header Bearer
//const headers = new Headers();
//headers.append('Authorization', `Bearer ${jwtToken}`);

const jwtToken =
    "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZG8yNzI0NDNAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3MDI2NTU1NDV9.tIneUnKmWzLcIVX7sXsn-SGJiDozKgT_PD6zr2HtW-iiXeL_dBd_2cQWXromigYcRFYWyGxw5rA37pM_hn5KKA";
    //"eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiY2hhbjk0MTcwQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik5ow6JuIHZpw6puIGzhuq9wIMSR4bq3dCIsImV4cCI6MTcwMjQ3NDM5N30.jG0E4Rx6USK3ClhFBgmReWMbdEY1Hb08gUGvAh852mFD0N1Ihtkqi4tHnt9jrIryuU8bHiC3xy65veWgC8mulQ";
    //"eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZG8yNzI0NDNAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiTmjDom4gdmnDqm4gbOG6r3AgxJHhurd0IiwiZXhwIjoxNzAyNjQ0NzY3fQ.jhV4OXc8SirO5G2wy93y-ObekFx7d2_xe0xdJrO828AITEg3etYh8wACL0JnqzYBU8ZqTI5YJZI9y9UWFzLSWA";
// Thiết lập header Bearer
const headers = new Headers();
headers.append("Authorization", `Bearer ${jwtToken}`);
headers.append("Content-Type", `application/json`);
headers.append("Access-Control-Allow-Origin", `*`);
headers.append("Access-Control-Allow-Methods", "POST,PATCH,OPTIONS,GET,PUT");

document.addEventListener("DOMContentLoaded", function () {
  // Khi trang được load, thiết lập giá trị mặc định cho startDate và endDate
  const startDateInput = document.getElementById("startDate");
  const endDateInput = document.getElementById("endDate");

  const currentDate = new Date();
  const sevenDaysAgo = new Date(currentDate);
  sevenDaysAgo.setDate(currentDate.getDate() - 7);

  startDateInput.valueAsDate = sevenDaysAgo; // Giá trị mặc định là ngày hôm nay
  endDateInput.valueAsDate = currentDate; // Giá trị mặc định là 7 ngày trước
  callDataCustomerByDate();
});

function callDataCustomerByDate() {
  $("#client-chart").remove();
  $("#div-client-chart").append(
    '<canvas class="chart" id="client-chart" height="300"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountCustomerAddedByDate";
  const startDateInput = document.getElementById("startDate").value;
  const endDateInput = document.getElementById("endDate").value;

  // Chuyển đổi chuỗi đầu vào thành đối tượng Date
  const startDate = new Date(startDateInput);
  const endDate = new Date(endDateInput);

  const startDateFormat = startDate.toISOString().substring(0, 10);
  const endDateFormat = endDate.toISOString().substring(0, 10);

  fetch(url + `?startDate=${startDateFormat}&endDate=${endDateFormat}`, {
    method: "GET",
    headers: headers,
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      const dateArray = [];
      const totalCustomersArray = [];
      const enterpriseCustomersArray = [];
      const individualCustomersArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalCustomersArray.push(item.totalCustomers);
        enterpriseCustomersArray.push(item.enterpriseCustomers);
        individualCustomersArray.push(item.individualCustomers);
      });

      const maxCustomers = Math.max(...totalCustomersArray);

      const mainChart = new Chart(document.getElementById("client-chart"), {
        type: "bar",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: coreui.Utils.getStyle("--cui-info"),
              data: totalCustomersArray,
            },
            {
              label: "Cá nhân",
              backgroundColor: coreui.Utils.getStyle("--cui-success"),
              data: individualCustomersArray,
            },
            {
              label: "Doanh nghiệp",
              backgroundColor: coreui.Utils.getStyle("--cui-danger"),
              data: enterpriseCustomersArray,
            },
          ],
        },
        options: {
          maintainAspectRatio: false,
          plugins: {
            legend: {
              display: true,
            },
          },
          scales: {
            x: {
              grid: {
                drawOnChartArea: false,
              },
            },
            y: {
              ticks: {
                beginAtZero: true,
                maxTicksLimit: 5,
                stepSize: Math.ceil(maxCustomers / 5),
                max: maxCustomers,
              },
            },
          },
        },
      });
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataCustomerByWeek() {
  $("#client-chart").remove();
  $("#div-client-chart").append(
    '<canvas class="chart" id="client-chart" height="300"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountCustomerAddedByWeek";
  const month = document.getElementById("weekpicker").value;

  // Chuyển đổi chuỗi đầu vào thành đối tượng Date
  const monthConvert = new Date(month);

  const monthFormat = monthConvert.toISOString().substring(0, 10);

  fetch(url + `?month=${monthFormat}`, {
    method: "GET",
    headers: headers,
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      const dateArray = [];
      const totalCustomersArray = [];
      const enterpriseCustomersArray = [];
      const individualCustomersArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalCustomersArray.push(item.totalCustomers);
        enterpriseCustomersArray.push(item.enterpriseCustomers);
        individualCustomersArray.push(item.individualCustomers);
      });

      const maxCustomers = Math.max(...totalCustomersArray);

      const mainChart = new Chart(document.getElementById("client-chart"), {
        type: "bar",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: coreui.Utils.getStyle("--cui-info"),
              data: totalCustomersArray,
            },
            {
              label: "Cá nhân",
              backgroundColor: coreui.Utils.getStyle("--cui-success"),
              data: individualCustomersArray,
            },
            {
              label: "Doanh nghiệp",
              backgroundColor: coreui.Utils.getStyle("--cui-danger"),
              data: enterpriseCustomersArray,
            },
          ],
        },
        options: {
          maintainAspectRatio: false,
          plugins: {
            legend: {
              display: true,
            },
          },
          scales: {
            x: {
              grid: {
                drawOnChartArea: false,
              },
            },
            y: {
              ticks: {
                beginAtZero: true,
                maxTicksLimit: 5,
                stepSize: Math.ceil(maxCustomers / 5),
                max: maxCustomers,
              },
            },
          },
        },
      });
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataCustomerByMonth() {
  $("#client-chart").remove();
  $("#div-client-chart").append(
    '<canvas class="chart" id="client-chart" height="300"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountCustomerAddedByMonth";
  const month = document.getElementById("monthpicker").value;

  // Chuyển đổi chuỗi đầu vào thành đối tượng Date
  const monthData = new Date(month);

  const monthConvert = monthData.toISOString().substring(0, 10);

  fetch(url + `?month=${monthConvert}`, {
    method: "GET",
    headers: headers,
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      return response.json();
    })
    .then((data) => {
      const dateArray = [];
      const totalCustomersArray = [];
      const enterpriseCustomersArray = [];
      const individualCustomersArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalCustomersArray.push(item.totalCustomers);
        enterpriseCustomersArray.push(item.enterpriseCustomers);
        individualCustomersArray.push(item.individualCustomers);
      });

      const maxCustomers = Math.max(...totalCustomersArray);

      const mainChart = new Chart(document.getElementById("client-chart"), {
        type: "bar",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: coreui.Utils.getStyle("--cui-info"),
              data: totalCustomersArray,
            },
            {
              label: "Cá nhân",
              backgroundColor: coreui.Utils.getStyle("--cui-success"),
              data: individualCustomersArray,
            },
            {
              label: "Doanh nghiệp",
              backgroundColor: coreui.Utils.getStyle("--cui-danger"),
              data: enterpriseCustomersArray,
            },
          ],
        },
        options: {
          maintainAspectRatio: false,
          plugins: {
            legend: {
              display: true,
            },
          },
          scales: {
            x: {
              grid: {
                drawOnChartArea: false,
              },
            },
            y: {
              ticks: {
                beginAtZero: true,
                maxTicksLimit: 5,
                stepSize: Math.ceil(maxCustomers / 5),
                max: maxCustomers,
              },
            },
          },
        },
      });
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}
