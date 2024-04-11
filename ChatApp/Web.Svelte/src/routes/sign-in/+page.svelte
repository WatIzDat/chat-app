<script>
	import { firebaseAuth } from '$lib/firebase.client';
	import { signInWithEmailAndPassword } from 'firebase/auth';
	import { goto } from '$app/navigation';
	import { authUser } from '$lib/authStore';

	let email;
	let password;

	let success = undefined;

	const signIn = () => {
		signInWithEmailAndPassword(firebaseAuth, email, password)
			.then((userCredential) => {
				authUser.set({
					uid: userCredential.user.uid,
					email: userCredential.user.email || ''
				});

				goto('/');
			})
			.catch((error) => {
				console.error(error);

				success = false;
			});
	};

	// async function signInWithEmail() {
	// 	await signInWithEmailAndPassword(auth, email, password)
	// 		.then((result) => {
	// 			const { user } = result;

	// 			session.set({
	// 				loggedIn: true,
	// 				user: {
	// 					displayName: user?.displayName,
	// 					email: user?.email,
	// 					uid: user?.uid
	// 				}
	// 			});

	// 			goto('/');
	// 		})
	// 		.catch((error) => {
	// 			return error;
	// 		});
	// }
</script>

<h1>Sign In</h1>

<!-- <form method="POST">
    <label>
        Email:
        <input name="email" type="email" />
    </label>

    <label>
        Password:
        <input name="password" type="password" />
    </label>

    <button type="submit">Submit</button>
</form> -->

<form on:submit|preventDefault={signIn}>
	<input bind:value={email} type="email" placeholder="Email" />
	<input bind:value={password} type="password" placeholder="Password" />

	<button type="submit">Sign In</button>
</form>
