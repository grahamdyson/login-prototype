const redirectUrl = new URLSearchParams(window.location.search).get("redirect");

if (redirectUrl) {
	document.getElementById("login-redirect").innerText = redirectUrl;
	document.getElementById("login-access-restricted").className = "has-redirect";
}

function login() {
	if (document.getElementById("login-form").reportValidity())
		fetch(
			"/authentication",
			createRequest(),
		)
		.then(handleResponse)
		.catch(error => showError());
}

function createRequest() {
	return {
		body:
			JSON.stringify({
				email: document.getElementById("email").value,
				password: document.getElementById("password").value,
			}),
		method:
			"POST",
		headers:
			{ "Content-Type": "application/json" },
	};
}

function handleResponse(
	response,
) {
	if (response.ok)
		location.href = redirectUrl || "/";
	else
		showError(
			response.status === 401
			&&
			"Log in failed, please check your email address and password."
		);
}

function showError(
	error,
) {
	document.getElementById("login-error").innerText =
		error
		||
		"Unexpected log in failure, please contact support.";
}
