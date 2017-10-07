"use strict"
var exec = require('child_process').exec

var azure = {}

azure.ps = function ps(script, args, callback) {
  var cmdline = ""
  for(var key in args) {
    cmdline += " -" + key + " \"" + args[key] + "\""
  }

  cmdline = "powershell.exe \"" + script + "\"" + cmdline 
  console.log(cmdline)

  exec(cmdline, function(err, stdout, stderr) {
     console.log(stdout)
     console.log(stderr)
     callback(err)
  })
}

azure.zip = function zip(websiteRoot, packagePath, hostName, callback) {
  this.ps(".\\scripts\\pack\\mkzip.ps1", {
    websiteRoot : websiteRoot,
    packagePath : packagePath,
    hostName : hostName,
  }, callback);
}

azure.wdp = function pack(sourcePackage, targetPackage, callback) {
  this.ps(".\\scripts\\pack\\mkwdp.ps1", {
    sourcePackage : sourcePackage,
    targetPackage : targetPackage
  }, callback)
}

module.exports = azure
