/* global Chart, coreui */

/**
 * --------------------------------------------------------------------------
 * CoreUI Boostrap Admin Template (v4.2.2): main.js
 * Licensed under MIT (https://coreui.io/license)
 * --------------------------------------------------------------------------
 */

const jwtToken =
  "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZG8yNzI0NDNAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3MDI4MjIxMDJ9.VvliL4HCT2lC3zjDE6vhgCQicsq9-0QQWWZtf6YmXn4YUcdmYJ3BKKEEhR3NND8MOMNVB1cCO6jP-RdNKt0qfw";
// "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiY2hhbjk0MTcwQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik5ow6JuIHZpw6puIGzhuq9wIMSR4bq3dCIsImV4cCI6MTcwMjczNzI5MX0.PQYeMr3vtDxa4BULYq-ay3b47i8zb5j89sxhbmJAW-sv28Tg8nr3zOKmzCYsxOCY38LMLfohsQM9u4gjeSaRJA";
//"eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiY2hhbjk0MTcwQGdtYWlsLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Ik5ow6JuIHZpw6puIGzhuq9wIMSR4bq3dCIsImV4cCI6MTcwMjQ3NDM5N30.jG0E4Rx6USK3ClhFBgmReWMbdEY1Hb08gUGvAh852mFD0N1Ihtkqi4tHnt9jrIryuU8bHiC3xy65veWgC8mulQ";
// Thiết lập header Bearer
const headers = new Headers();
headers.append("Authorization", `Bearer ${jwtToken}`);
headers.append("Content-Type", `application/json`);
headers.append("Access-Control-Allow-Origin", `*`);
headers.append("Access-Control-Allow-Methods", "POST,PATCH,OPTIONS,GET,PUT");

// Disable the on-canvas tooltip
Chart.defaults.pointHitDetectionRadius = 1;
Chart.defaults.plugins.tooltip.enabled = false;
Chart.defaults.plugins.tooltip.mode = "index";
Chart.defaults.plugins.tooltip.position = "nearest";
Chart.defaults.plugins.tooltip.external = coreui.ChartJS.customTooltips;
Chart.defaults.defaultFontColor = "#646470";

function callDataPContractByDate() {
  $("#pContract-chart").remove();
  $("#div-pContract-chart").append(
    ' <canvas class="chart" id="pContract-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountPContractCreatedByDate";

  const endDateInput = new Date();
  const startDateInput = new Date(endDateInput);
  startDateInput.setDate(endDateInput.getDate() - 7);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumPContract").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(document.getElementById("pContract-chart"), {
        type: "line",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: "transparent",
              borderColor: "rgba(255,255,255,.55)",
              pointBackgroundColor: coreui.Utils.getStyle("--cui-primary"),
              data: totalsArray,
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
              min: minValue + 2,
              max: maxValue - 2,
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
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataPContractByWeek() {
  $("#pContract-chart").remove();
  $("#div-pContract-chart").append(
    ' <canvas class="chart" id="pContract-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountPContractCreatedByWeek";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumPContract").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(document.getElementById("pContract-chart"), {
        type: "line",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: "transparent",
              borderColor: "rgba(255,255,255,.55)",
              pointBackgroundColor: coreui.Utils.getStyle("--cui-primary"),
              data: totalsArray,
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
              min: Math.ceil(minValue / 2),
              max: Math.ceil(maxValue * 2),
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
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataPContractByMonth() {
  $("#pContract-chart").remove();
  $("#div-pContract-chart").append(
    ' <canvas class="chart" id="pContract-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountPContractCreatedByMonth";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumPContract").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(document.getElementById("pContract-chart"), {
        type: "line",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: "transparent",
              borderColor: "rgba(255,255,255,.55)",
              pointBackgroundColor: coreui.Utils.getStyle("--cui-primary"),
              data: totalsArray,
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
              min: Math.ceil(minValue / 2),
              max: Math.ceil(maxValue * 2),
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
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataPContractWaitCusByDate() {
  $("#pContractWaitCus-chart").remove();
  $("#div-pContractWaitCus-chart").append(
    ' <canvas class="chart" id="pContractWaitCus-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountPContractWaitCusByDate";

  const endDateInput = new Date();
  const startDateInput = new Date(endDateInput);
  startDateInput.setDate(endDateInput.getDate() - 7);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      let sum = 0;
      sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumPContractWaitCus").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(
        document.getElementById("pContractWaitCus-chart"),
        {
          type: "line",
          data: {
            labels: dateArray,
            datasets: [
              {
                label: "Tổng",
                backgroundColor: "transparent",
                borderColor: "rgba(255,255,255,.55)",
                pointBackgroundColor: coreui.Utils.getStyle("--cui-warning"),
                data: totalsArray,
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
                min: minValue + 2,
                max: maxValue - 2,
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
        }
      );
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataPContractWaitCusByWeek() {
  $("#pContractWaitCus-chart").remove();
  $("#div-pContractWaitCus-chart").append(
    ' <canvas class="chart" id="pContractWaitCus-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountPContractWaitCusByWeek";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      let sum = 0;
      sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumPContractWaitCus").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(
        document.getElementById("pContractWaitCus-chart"),
        {
          type: "line",
          data: {
            labels: dateArray,
            datasets: [
              {
                label: "Tổng",
                backgroundColor: "transparent",
                borderColor: "rgba(255,255,255,.55)",
                pointBackgroundColor: coreui.Utils.getStyle("--cui-warning"),
                data: totalsArray,
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
                min: Math.ceil(minValue / 2),
                max: Math.ceil(maxValue * 2),
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
        }
      );
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataPContractWaitCusByMonth() {
  $("#pContractWaitCus-chart").remove();
  $("#div-pContractWaitCus-chart").append(
    ' <canvas class="chart" id="pContractWaitCus-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountPContractWaitCusByMonth";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      let sum = 0;
      sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumPContractWaitCus").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(
        document.getElementById("pContractWaitCus-chart"),
        {
          type: "line",
          data: {
            labels: dateArray,
            datasets: [
              {
                label: "Tổng",
                backgroundColor: "transparent",
                borderColor: "rgba(255,255,255,.55)",
                pointBackgroundColor: coreui.Utils.getStyle("--cui-warning"),
                data: totalsArray,
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
                min: Math.ceil(minValue / 2),
                max: Math.ceil(maxValue * 2),
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
        }
      );
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataDContractByDate() {
  $("#dContract-chart").remove();
  $("#div-dContract-chart").append(
    ' <canvas class="chart" id="dContract-chart" height="70"></canvas>'
  );
  const url =
    "https://localhost:7286/api/ExecProcedure/CountDContractCreatedByDate";

  const endDateInput = new Date();
  const startDateInput = new Date(endDateInput);
  startDateInput.setDate(endDateInput.getDate() - 7);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumDContract").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(document.getElementById("dContract-chart"), {
        type: "line",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: "transparent",
              borderColor: "rgba(255,255,255,.55)",
              pointBackgroundColor: coreui.Utils.getStyle("--cui-success"),
              data: totalsArray,
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
              min: Math.ceil(minValue / 2),
              max: Math.ceil(maxValue * 2),
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
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataDContractByWeek() {
  $("#dContract-chart").remove();
  $("#div-dContract-chart").append(
    ' <canvas class="chart" id="dContract-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountDContractCreatedByWeek";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumDContract").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(document.getElementById("dContract-chart"), {
        type: "line",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: "transparent",
              borderColor: "rgba(255,255,255,.55)",
              pointBackgroundColor: coreui.Utils.getStyle("--cui-success"),
              data: totalsArray,
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
              min: Math.ceil(minValue / 2),
              max: Math.ceil(maxValue * 2),
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
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataDContractByMonth() {
  $("#dContract-chart").remove();
  $("#div-dContract-chart").append(
    ' <canvas class="chart" id="dContract-chart" height="70"></canvas>'
  );

  const url =
    "https://localhost:7286/api/ExecProcedure/CountDContractCreatedByMonth";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumDContract").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(document.getElementById("dContract-chart"), {
        type: "line",
        data: {
          labels: dateArray,
          datasets: [
            {
              label: "Tổng",
              backgroundColor: "transparent",
              borderColor: "rgba(255,255,255,.55)",
              pointBackgroundColor: coreui.Utils.getStyle("--cui-success"),
              data: totalsArray,
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
              min: Math.ceil(minValue / 2),
              max: Math.ceil(maxValue * 2),
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
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

// eslint-disable-next-line no-unused-vars

function callDataUnEffectByDate() {
  $("#dContractUneffect-chart").remove();
  $("#div-dContractUneffect-chart").append(
    ' <canvas class="chart" id="dContractUneffect-chart" height="70"></canvas>'
  );
  const url = "https://localhost:7286/api/ExecProcedure/CountUnEffectByDate";

  const endDateInput = new Date();
  const startDateInput = new Date(endDateInput);
  startDateInput.setDate(endDateInput.getDate() - 7);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumUnEffect").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(
        document.getElementById("dContractUneffect-chart"),
        {
          type: "line",
          data: {
            labels: dateArray,
            datasets: [
              {
                label: "Tổng",
                backgroundColor: "transparent",
                borderColor: "rgba(255,255,255,.55)",
                pointBackgroundColor: coreui.Utils.getStyle("--cui-danger"),
                data: totalsArray,
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
                min: Math.ceil(minValue / 2),
                max: Math.ceil(maxValue * 2),
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
        }
      );
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataUnEffectByWeek() {
  $("#dContractUneffect-chart").remove();
  $("#div-dContractUneffect-chart").append(
    ' <canvas class="chart" id="dContractUneffect-chart" height="70"></canvas>'
  );

  const url = "https://localhost:7286/api/ExecProcedure/CountUnEffectByWeek";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumUnEffect").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(
        document.getElementById("dContractUneffect-chart"),
        {
          type: "line",
          data: {
            labels: dateArray,
            datasets: [
              {
                label: "Tổng",
                backgroundColor: "transparent",
                borderColor: "rgba(255,255,255,.55)",
                pointBackgroundColor: coreui.Utils.getStyle("--cui-danger"),
                data: totalsArray,
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
                min: Math.ceil(minValue / 2),
                max: Math.ceil(maxValue * 2),
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
        }
      );
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

function callDataUnEffectByMonth() {
  $("#dContractUneffect-chart").remove();
  $("#div-dContractUneffect-chart").append(
    ' <canvas class="chart" id="dContractUneffect-chart" height="70"></canvas>'
  );

  const url = "https://localhost:7286/api/ExecProcedure/CountUnEffectByMonth";

  const month = new Date();

  var monthFormat = month.toISOString().substring(0, 10);

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
      const totalsArray = [];

      data.result.forEach((item) => {
        // Lưu giá trị từ các trường vào các mảng tương ứng
        dateArray.push(item.date);
        totalsArray.push(item.totals);
      });

      const sum = totalsArray.reduceRight((acc, cur) => acc + cur, 0);

      document.getElementById("sumUnEffect").innerHTML = sum;

      const maxValue = Math.max(...totalsArray);
      const minValue = Math.min(...totalsArray);

      const cardChart1 = new Chart(
        document.getElementById("dContractUneffect-chart"),
        {
          type: "line",
          data: {
            labels: dateArray,
            datasets: [
              {
                label: "Tổng",
                backgroundColor: "transparent",
                borderColor: "rgba(255,255,255,.55)",
                pointBackgroundColor: coreui.Utils.getStyle("--cui-danger"),
                data: totalsArray,
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
                min: Math.ceil(minValue / 2),
                max: Math.ceil(maxValue * 2),
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
        }
      );
    })
    .catch((error) => {
      console.error("Fetch error:", error);
    });
}

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
  callDataPContractByDate();
  callDataDContractByDate();
  callDataPContractWaitCusByDate();
  callDataUnEffectByDate();
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
