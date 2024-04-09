import { getAuth, signInWithEmailAndPassword } from 'firebase/auth';
import { redirect } from '@sveltejs/kit';

let firebaseApp;

export async function load(event) {
	firebaseApp = event.locals.firebaseApp;
}

export const actions = {
	default: async ({ cookies, request }) => {
		const auth = getAuth(firebaseApp);

		const data = await request.formData();

		await signInWithEmailAndPassword(auth, data.get('email'), data.get('password'));

		throw redirect(303, '/');
	}
};
