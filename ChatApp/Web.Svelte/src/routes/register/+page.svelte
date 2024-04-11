<script>
	import { goto } from '$app/navigation';

	import { createUserWithEmailAndPassword } from 'firebase/auth';
	import { firebaseAuth } from '$lib/firebase';

	let email;
	let password;

	let success = undefined;

	const register = () => {
		createUserWithEmailAndPassword(firebaseAuth, email, password)
			.then((userCredentials) => {
				console.log(userCredentials);

				goto('/login');
			})
			.catch((error) => {
				console.error(error);

				success = false;
			});
	};
</script>

<h1>Register</h1>

<form on:submit|preventDefault={register}>
	<input bind:value={email} type="email" placeholder="Email" />
	<input bind:value={password} type="password" placeholder="Password" />

	<button type="submit">Register</button>
</form>

<!-- <form method="POST">
    <label>
        Email:
        <input name="email" type="email" />
    </label>

    <label>
        Display Name:
        <input name="name" />
    </label>

    <label>
        Password:
        <input name="password" type="password" />
    </label>

    <button type="submit">Submit</button>
</form> -->
