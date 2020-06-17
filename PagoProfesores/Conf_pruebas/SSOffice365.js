$(function () {
    $(window).load(function () {
        var url = window.location.href
        var arr = url.split("/");
        var domain = arr[0] + "//" + arr[2]

        if (window.location.hash) {
            var hash = window.location.hash.substring(1); //Puts hash in variable, and removes the # character
            window.location.href = domain + "/SSOffice365/VerifyToken/?" + hash;
        } else {
            signIn();
        }
    });
});

  var ADAL = new AuthenticationContext({
      instance: 'https://login.microsoftonline.com/',
      tenant: 'common', //COMMON OR YOUR TENANT ID

     // clientId: '48cb2fe3-ca43-419f-a06c-0e261465dea7', //REPLACE WITH YOUR CLIENT ID 
 clientId: '455e86f8-7469-4ff3-8185-2f47c7fba07b', 	 

	  
	   //redirectUri: 'https://pagoprofesores-p.azurewebsites.net/ssoOffice365.html',

	  redirectUri: 'https://pagoprofesores-test.redanahuac.mx/ssoOffice365.html',
     
      callback: userSignedIn,
      popUp: false
  });


function signIn() {
    ADAL.login();
}

function userSignedIn(err, token) {
    console.log('userSignedIn called');
    if (!err) {
        console.log("token: " + token);
        showWelcomeMessage();
    }
    else {
        console.error("error: " + err);
    }
}

function showWelcomeMessage() {
    var user = ADAL.getCachedUser();
    var divWelcome = document.getElementById('WelcomeMessage');
    divWelcome.innerHTML = "Welcome " + user.profile.name;
}

