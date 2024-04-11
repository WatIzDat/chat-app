<script>
	import { onAuthStateChanged } from 'firebase/auth';
	import { onMount } from 'svelte';
	import { firebaseAuth } from '$lib/firebase.client';
	import { redirect } from '@sveltejs/kit';

	// onMount(() => {
	// 	onAuthStateChanged(firebaseAuth, (user) => {
	// 		if (!user) {
	// 			throw redirect(302, '/sign-in');
	// 		}
	// 	});
	// });

	export let data;
</script>

<h1>{data.name}: {data.id}</h1>
