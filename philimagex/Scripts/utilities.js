﻿// Compiled utilities
// By: Harold Glenn Minerva 

var utilities = (function () {
    return {
        // Get URL Parameter Name
        getParameterByName: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        },

        // Format Number
        formatNumber: function (num, dec, thou, pnt, curr1, curr2, n1, n2) {
            var x = Math.round(num * Math.pow(10, dec));
            if (x >= 0) n1 = n2 = '';
            var y = ('' + Math.abs(x)).split('');
            var z = y.length - dec;
            if (z < 0) z--;
            for (var i = z; i < 0; i++) y.unshift('0');
            if (z < 0) z = 1; y.splice(z, 0, pnt);
            if (y[0] == pnt) y.unshift('0');
            while (z > 3) {
                z -= 3;
                y.splice(z, 0, thou);
            }
            var r = curr1 + n1 + y.join('') + n2 + curr2;
            return r;
        },

        // Get Table (Jquery.datatables) Data by row and column - By: HGM
        getTableData: function (table, id, columnIdName, columnName) {
            var data = table.dataTable().fnGetData();

            var d = "";
            for (i = 0; i < 10; i++) {
                if (data[i][columnIdName] == id) {
                    d = data[i][columnName];
                    break;
                }
            }
            return d;
        },

        // Get Current Date MM/DD/YYYY
        getCurrentDate: function () {
            var currentDate = new Date()
            var day = currentDate.getDate()
            var month = currentDate.getMonth() + 1
            var year = currentDate.getFullYear()

            return month + "/" + day + "/" + year;
        },

        // Get Current Time HH:MM
        getCurrentTime: function () {
            var currentDate = new Date()
            var hour = (currentDate.getHours() < 10 ? '0' : '') + currentDate.getHours();
            var min = (currentDate.getMinutes() < 10 ? '0' : '') + currentDate.getMinutes();
            return hour + ':' + min
        },

        // Get DICOM File 
        getPatientInfo: function(ThePath,GivenPatientId){  
            var returnFlag = false;
            var foundFlag = false;

            var fso;
            fso = new ActiveXObject("Scripting.FileSystemObject");

            if(ThePath.length>0 && fso.FolderExists(ThePath))
            {
                var foldName = fso.GetFolder(ThePath);
                var eNum = new Enumerator(foldName.Files);
                var Files = "";
                var PatientId = "";
                var PatientName = "";
                var BirthDate = "";
                var Age = "";
                var Gender = "";
                for(var i=0;!eNum.atEnd();eNum.moveNext())
                {
                    var DICOMFile = ThePath + "\\" + eNum.item().Name;
                    var iStream = fso.OpenTextFile(DICOMFile, 1, false);
                    var str = "";
                    var Return
                    var FileName = "";

                    if (returnFlag == false) {
                        PatientId = "";
                        PatientName = "";
                        BirthDate = "";
                        Age = "";
                        Gender = "";
                    }

                    while (!iStream.AtEndOfStream) {
                        str = iStream.ReadLine();

                        // Dicom Filename
                        if (str.indexOf("DICOM file:") > -1) {
                            FileName = str.substring(12).replace(/\\/g, "\\\\");
                        }
                        
                        // Patient’s Name
                        if (str.indexOf("Patient’s Name") > -1) {
                            if (returnFlag == false) {
                                PatientName = str.substring(74).replace(/\s+/g, '').replace(/\^/g, ' ');
                            }
                        }

                        // Patient ID
                        if (str.indexOf("Patient ID") > -1) {
                            PatientId = str.substring(74).replace(/\s+/g, '');
                            if (GivenPatientId == PatientId) {
                                if (Files == "") {
                                    Files = FileName;
                                } else {
                                    Files = Files + ";" + FileName;
                                }
                                foundFlag = true;
                            } 
                        }

                        // Patient's Birth Date
                        if (str.indexOf("Patient's Birth Date") > -1) {
                            if (returnFlag == false) {
                                BirthDate = str.substring(74).replace(/\s+/g, '');
                            }  
                        }

                        // Patient's Sex
                        if (str.indexOf("Patient's Sex") > -1) {
                            if (returnFlag == false) {
                                Gender = str.substring(74).replace(/\s+/g, '');
                            }
                        }

                        // Patient's Age
                        if (str.indexOf("Patient's Age") > -1) {
                            if (returnFlag == false) {
                                Age = str.substring(74).replace(/\s+/g, '');
                            }
                        }
                    }
                    iStream.Close();
                    i++;

                    if (foundFlag == true) {
                        returnFlag = true;
                    }
                }

                if (returnFlag == true) {
                    var PatientInfo = {
                        "DICOMFileName": Files,
                        "PatientName": PatientName,
                        "PatientId": PatientId,
                        "BirthDate": BirthDate,
                        "Gender": Gender,
                        "Age": Age
                    };
                    return PatientInfo;
                }
            }
            var EmptyPatientInfo = {
                "DICOMFileName": "NA",
                "PatientName": "NA",
                "PatientId": "NA",
                "BirthDate": "NA",
                "Gender": "NA",
                "Age": "NA"
            };
            return EmptyPatientInfo;
        },

        // Using Windows Script Host Shell ActiveX to run cmd commands
        runBatchFile: function (which) {
            var WshShell;
            WshShell = new ActiveXObject("WScript.Shell");
            WshShell.Run(which);
        },

        // Return a link from a data
        returnLink: function (data) {
            var n = data.indexOf(")");
            var Link = data.substring(1, n);
            var String = data.substring(n + 1, data.length);
            return '<a href="' + Link + '">' + String + '</a>';
        },

        // Round to 2 decimal places
        round2Number: function (num) {
            return Math.round(num * 100) / 100;
        }
    };
}());