<script>
	import { onMount } from 'svelte';
	import { firebaseAuth } from '$lib/firebase.client';
	import { signOut } from 'firebase/auth';
	import { authUser } from '$lib/authStore';
	import { goto } from '$app/navigation';

	onMount(() => {
		signOut(firebaseAuth)
			.then(() => {
				$authUser = undefined;

				goto('/');
			})
			.catch((error) => {
				console.error(error);
			});
	});
</script>
