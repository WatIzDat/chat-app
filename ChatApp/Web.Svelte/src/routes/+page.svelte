<!-- <script>
    import { onMount } from 'svelte';
    import { auth } from '$lib/auth';
    import { getAuth, onAuthStateChanged } from 'firebase/auth';
    import { browser } from '$app/environment';

    // export let data;

    let name;

    $: {
        // onAuthStateChanged(auth, (user) => {
    	//         if (user) {
    	// 	        name = user.displayName;
        //             console.log(user);
    	//         } else {
    	// 	        name = 'no name';
        //             console.log("this was null");
    	//         }
        // })

        console.log(auth.currentUser);
    }

    // onMount(() => {
    //     const auth = getAuthWrapper();

    //     onAuthStateChanged(auth, (user) => {
    //         console.log(user);
	//         // if (user) {
	// 	    //     name = user.displayName;
	//         // } else {
	// 	    //     name = 'no name';
	//         // }
    //     })
    // })
</script> -->

<script>
	import { authUser } from '$lib/authStore';
	import { onAuthStateChanged } from 'firebase/auth';
	import { onMount } from 'svelte';
	import { firebaseAuth } from '$lib/firebase.client';
	import { get } from 'svelte/store';

	let email = 'Loading...';

	onMount(() => {
		onAuthStateChanged(firebaseAuth, (user) => {
			if (user) {
				console.log('there is a user');

				email = user.email;

				authUser.set({
					uid: user.uid,
					email: user.email || ''
				});

				console.log(get(authUser));
			}
		});

		// const unsubscribe = authUser.subscribe((user) => {
		// 	if (user) {
		// 		email = user.email;
		// 	} else {
		// 		email = 'no email';
		// 	}
		// });

		// unsubscribe();
	});
</script>

<h1>Welcome to SvelteKit, {email}</h1>
<p>Visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to read the documentation</p>
