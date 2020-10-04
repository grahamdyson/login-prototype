const redirectUrl = new URLSearchParams(window.location.search).get("redirect");

if (redirectUrl) {
	document.getElementById("login-redirect").innerText = redirectUrl;
	document.getElementById("login-access-restricted").className = "has-redirect";
}

document.getElementById("login-form")
.addEventListener(
	"submit",
	event => {
		event.preventDefault();

		fetch(
			"/authentication",
			createRequestFromFormData(
				new FormData(event.target),
			),
		)
		.then(handleResponse)
		.catch(error => showError());
	},
);

function createRequestFromFormData(
	fromData,
) {
	return {
		body:
			JSON.stringify({
				email: fromData.get("email"),
				password: fromData.get("password"),
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
