import { getAuth } from 'firebase/auth';

export async function load(event) {
	const auth = getAuth(event.locals.firebaseApp);
	const user = auth.currentUser;

	if (user) {
		return { name: user.displayName };
	} else {
		return { name: 'no name' };
	}
}
