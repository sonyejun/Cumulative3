
// This function attaches a timer object to the input window.
// When the timer expires (300ms), the search executes.
// Prevents a search on each key up for fast typers.
function _ListTeachers(d) {
	if (d.timer) clearTimeout(d.timer);
	d.timer = setTimeout(function () { ListTeachers(d.value); }, 300);
};
//The actual List Teachers Method.
function ListTeachers(SearchKey) {

	var URL = "https://localhost:44397/api/TeacherData/ListTeachers/" + SearchKey;

	var rq = new XMLHttpRequest();
	rq.open("GET", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4 && rq.status == 200) {
			//request is successful and the request is finished


			var teachers = JSON.parse(rq.responseText)
			var listteachers = document.getElementById("listteachers");
			listteachers.innerHTML = "";

			//renders content for each teacher pulled from the API call
			for (var i = 0; i < teachers.length; i++) {
				var row = document.createElement("div");
				row.classList = "listitem row";
				var col = document.createElement("col");
				col.classList = "col-md-12";
				var link = document.createElement("a");
				link.href = "/Teacher/Show/" + teachers[i].TeacherId;
				link.innerHTML = teachers[i].TeacherFname + " " + teachers[i].TeacherLname;

				col.appendChild(link);
				row.appendChild(col);
				listteachers.appendChild(row);

			}
		}

	}
	//POST information sent through the .send() method
	rq.send();
};


// Usually Validation functions for Add and Update are separated.
// You can run into situations where information added is no longer updated, or vice versa
// However, as an example, validation is consolidated into 'ValidateTeacher'
// This is so that both Ajax and Non Ajax techniques can utilize the same client-side validation logic.
function ValidateTeacher() {
	var IsValid = true;
	var ErrorMsg = "";
	var ErrorBox = document.getElementById("ErrorBox");
	var TeacherFname = document.getElementById('TeacherFname').value;
	var TeacherLname = document.getElementById('TeacherLname').value;
	var EmployeeNumber = document.getElementById('EmployeeNumber').value;
	var Salary = document.getElementById('Salary').value;

	//First Name is two or more characters
	if (TeacherFname.length < 2) {
		IsValid = false;
		ErrorMsg += "First Name Must be 2 or more characters.<br>";
	};

	//Last Name is two or more characters
	if (TeacherLname.length < 2) {
		IsValid = false;
		ErrorMsg += "Last Name Must be 2 or more characters.<br>";
	};

	//Employee Number is two or more characters
	if (EmployeeNumber.length < 2) {
		IsValid = false;
		ErrorMsg += "Last Name Must be 2 or more characters.<br>";
	}

	//Email is valid pattern
	if (!ValidateSalary(Salary)) {
		IsValid = false;
		ErrorMsg += "Please Enter a valid Salary.<br>";
	}

	if (!IsValid) {
		ErrorBox.style.display = "block";
		ErrorBox.innerHTML = ErrorMsg;

	} else {
		ErrorBox.style.display = "none";
		ErrorBox.innerHTML = "";
	}


	return IsValid;
};

// create teacher
function AddTeacher() {

	//check for validation straight away
	var IsValid = ValidateTeacher();
	if (!IsValid) return;

	//goal: send a request which looks like this:
	//with POST data of teahcername, EmployeeNumber, Salary
	var URL = "https://localhost:44397/api/TeacherData/AddTeacher/";

	var rq = new XMLHttpRequest();

	var TeacherFname = document.getElementById('TeacherFname').value;
	var TeacherLname = document.getElementById('TeacherLname').value;
	var EmployeeNumber = document.getElementById('EmployeeNumber').value;
	var Salary = document.getElementById('Salary').value;

	var TeacherData = {
		"TeacherFname": TeacherFname,
		"TeacherLname": TeacherLname,
		"EmployeeNumber": EmployeeNumber,
		"Salary": Salary
	};


	rq.open("POST", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");//Send in Json format
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4) {
			if (rq.status == 200 || rq.status == 204) {
				console.log("Teacher added successfully!");
				window.location.href = "/Teacher/List";
			} else {
				console.error("HTTP error: " + rq.status);
				console.error(rq.responseText);
			}
		}

	}
	//POST information sent through the .send() method
	rq.send(JSON.stringify(TeacherData));
}


// update teacher
function UpdateTeacher(id) {

	//check for validation straight away
	var IsValid = ValidateTeacher();
	if (!IsValid) return;

	//goal: send a request which looks like this:
	//with POST data of teahcername, EmployeeNumber, Salary
	var URL = "https://localhost:44397/api/TeacherData/UpdateTeacher/"+id;
	
	var rq = new XMLHttpRequest();

	var TeacherFname = document.getElementById('TeacherFname').value;
	var TeacherLname = document.getElementById('TeacherLname').value;
	var EmployeeNumber = document.getElementById('EmployeeNumber').value;
	var Salary = document.getElementById('Salary').value;

	var TeacherData = {
		"TeacherFname": TeacherFname,
		"TeacherLname": TeacherLname,
		"EmployeeNumber": EmployeeNumber,
		"Salary": Salary
	};


	rq.open("POST", URL, true);
	rq.setRequestHeader("Content-Type", "application/json"); //Send in Json format
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4) {
			if (rq.status == 200 || rq.status == 204) {
				console.log("Teacher added successfully!");
				window.location.href = "/Teacher/Show/"+ id;
			} else {
				console.error("HTTP error: " + rq.status);
				console.error(rq.responseText);
			}
		}

	}
	//POST information sent through the .send() method
	rq.send(JSON.stringify(TeacherData));
}

// Delete Teacher
function DeleteTeacher(id) {

	//URL
	var URL = `https://localhost:44397/api/TeacherData/DeleteTeacher/${id}`;

	var rq = new XMLHttpRequest();

	rq.open("POST", URL, true);

	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4) {
			if (rq.status == 200 || rq.status == 204) {
				console.log("Teacher added successfully!");
				window.location.href = "/Teacher/List";
			} else {
				console.error("HTTP error: " + rq.status);
				console.error(rq.responseText);
			}

		}
	};

	//POST information sent through the .send() method
	rq.send();
};

// Teacher Data Show
function ShowTeacher(id) {
	//URL
	var URL = `https://localhost:44397/api/TeacherData/FindTeacher/${id}`;

	var rq = new XMLHttpRequest();

	rq.open("GET", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");//Send in Json format
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4) {
			if (rq.status == 200) {
				console.log("Teacher added successfully!");

				// Parse the JSON data received from the server
				var teacherData = JSON.parse(rq.responseText);

				// Format the date (DD-MM-YYYY)
				var dateObject = new Date(teacherData.Hiredate);
				var formattedHireDate = `${dateObject.getDate()}-${dateObject.getMonth() + 1}-${dateObject.getFullYear()}`;

				// Format the salary (two decimal places)
				var formattedSalary = parseFloat(teacherData.Salary).toFixed(2);

				document.getElementById('deleteLink').href = `Teacher/Update/${teacherData.TeacherId}`;
				document.getElementById('updateLink').href = `Teacher / DeleteConfirm /${teacherData.TeacherId}`;
				document.getElementById('name').innerText = teacherData.TeacherFname + teacherData.TeacherLname
				document.getElementById('EmpNum').innerText = `Employee Number: ${teacherData.EmployeeNumber}`
				document.getElementById('date').innerText = `Hiredate: ${formattedHireDate}`
				document.getElementById('sal').innerText = `Salary: ${formattedSalary}`
				
				
			} else {
				console.error("HTTP error: " + rq.status);
				console.error(rq.responseText);
				window.location.href = "/Home/Error";
			}

		}
	};

	//GET information sent through the .send() method
	rq.send();
}

function ValidateSalary(salary) {
	const re = /^\d+(\.\d{1,2})?$/;
	return re.test(String(salary));
}