import { redirect } from '@sveltejs/kit';
import { getAuth, createUserWithEmailAndPassword, updateProfile } from 'firebase/auth';

let firebaseApp;

export async function load(event) {
	firebaseApp = event.locals.firebaseApp;
}

export const actions = {
	default: async ({ cookies, request }) => {
		const auth = getAuth(firebaseApp);

		const data = await request.formData();

		const userCredential = await createUserWithEmailAndPassword(
			auth,
			data.get('email'),
			data.get('password')
		);

		const user = userCredential.user;

		updateProfile(user, {
			displayName: data.get('name')
		});

		throw redirect(303, '/');
	}
};
