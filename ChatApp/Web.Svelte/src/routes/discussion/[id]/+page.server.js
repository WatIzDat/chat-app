export async function load({ params }) {
	const discussion = await getDiscussion(params);

	console.log(discussion);

	return discussion;
}

async function getDiscussion(params) {
	const url = new URL('http://localhost:8080/discussions/get-discussion-by-id');

	url.search = new URLSearchParams({
		id: params.id
	});

	console.log(url.toString());

	const response = await fetch(url.toString());

	return await response.json();
}
