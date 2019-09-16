var express = require("express");
var app = express();
var http = require("http").createServer(app);
var moment = require("moment");

//get 3rd argument since first two are related to this process and user defined are starting from index 2
var port = process.argv.length > 2 ? parseInt(process.argv[2]) : 3000;
var host = "localhost";

var startTime = null;
var status = "offline";
var statusLastChangedDate = null;
var downloadSpeed = null;
var uploadSpeed = null;
var uploadSpeedThreshold = null;
var downloadSpeedThreshold = null;
var offset = 10;

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.get("/", (req, res) => {
  res.send({
      success: true,
      host: host,
      port: port,
      status: status,
      startTime: startTime.toDate()
  });
});

app.get("/status", (req, res) => {
  res.send({
    success: true,
    host: host,
    port: port,
    status: status,
    statusLastChangedDate: statusLastChangedDate,
    statusActivity: statusLastChangedDate.from(moment())
  });
});

app.post("/turnOn", (req, res) => {
  status = "online";
  statusLastChangedDate = moment();
  uploadSpeed = Math.floor(Math.random() * (uploadSpeedThreshold + offset) * 1.00);
  downloadSpeed = Math.floor(Math.random() * (downloadSpeedThreshold + offset) * 1.00);
  res.send(getNodeData(uploadSpeed, uploadSpeedThreshold, downloadSpeed, downloadSpeedThreshold));
});

app.post("/turnOff", (req, res) => {
  status = "offline";
  statusLastChangedDate = moment();
  res.send({
    success: true,
    host: host,
    port: port,
    status: status,
    statusLastChangedDate: statusLastChangedDate.toDate(),
    statusActivity: statusLastChangedDate.from(moment()),
  });
});

app.get("/telemetryMetrics", (req, res) => {
  if (status == "online") {
    uploadSpeed = Math.floor(Math.random() * 160.00);
    downloadSpeed = Math.floor(Math.random() * 110.00);

    res.send(getNodeData(uploadSpeed, uploadSpeedThreshold, downloadSpeed, downloadSpeedThreshold));
  } else {
    res.send(getNodeData(null, null, null, null));
  }
});

app.post("/threshold", (req, res) => {
  if (req.body) {
    uploadSpeedThreshold = req.body.uploadThreshold
      ? parseFloat(req.body.uploadThreshold).toFixed(2)
      : uploadSpeedThreshold;
    downloadSpeedThreshold = req.body.downloadThreshold
      ? parseFloat(req.body.downloadThreshold).toFixed(2)
      : downloadSpeedThreshold;

    res.send(getNodeData(uploadSpeed, uploadSpeedThreshold, downloadSpeed, downloadSpeedThreshold));
  } else {
    res.status(500).send({
      success: false,
      message: "UploadThreshold and/or downloadThreshold are undefined."
    });
  }
});

function getNodeData(uploadSpeedParam, uploadSpeedThresholdParam, downloadSpeedParam, downloadSpeedThresholdParam){
  return {
    success: true,
    host: host,
    port: port,
    status: status,
    activityTime: startTime.from(moment(), true),
    statusLastChangedDate: statusLastChangedDate.toDate(),
    statusActivity: statusLastChangedDate.from(moment()),

    uploadSpeed: uploadSpeedParam,
    uploadSpeedThreshold: uploadSpeedThresholdParam,
    uploadThresholdStatus: uploadSpeedThresholdParam != null && uploadSpeedParam > uploadSpeedThresholdParam ? 1 : 0,
    uploadThresholdStatusMessage:
    uploadSpeedThresholdParam != null && uploadSpeedParam > uploadSpeedThresholdParam
            ? "Upload speed exceed maximum limits"
            : "OK",

    downloadSpeed: downloadSpeedParam,
    downloadSpeedThreshold:downloadSpeedThresholdParam,
    downloadThresholdStatus: downloadSpeedThresholdParam!= null && downloadSpeedParam > downloadSpeedThresholdParam ? 1 : 0,
    downloadThresholdStatusMessage:
    downloadSpeedThresholdParam!= null && downloadSpeedParam > downloadSpeedThresholdParam
          ? "Download speed exceed maximum limits"
          : "OK"
  }
}

http.listen(port, function() {
  startTime = moment();
  statusLastChangedDate = moment();
  console.log("Listening on " + host + " " + port);
});
