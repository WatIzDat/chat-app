import { getApps, initializeApp } from 'firebase/app';
import { getAuth } from 'firebase/auth';
// import { browser } from '$app/environment';

// export let db;
// export let app;
// export let auth;

const firebaseConfig = {
	apiKey: 'AIzaSyDjZtSoHT9oUphzi0tCMxrASftJMTS1NtI',
	authDomain: 'chat-app-59e5b.firebaseapp.com',
	projectId: 'chat-app-59e5b',
	storageBucket: 'chat-app-59e5b.appspot.com',
	messagingSenderId: '664884418847',
	appId: '1:664884418847:web:6a292a9dd524c3b58e382a'
};

let firebaseApp;

if (!getApps().length) {
	firebaseApp = initializeApp(firebaseConfig);
}

const firebaseAuth = getAuth(firebaseApp);

export { firebaseApp, firebaseAuth };

// export function initializeFirebase() {
// 	if (!browser) {
// 		throw new Error('Cannot use the Firebase client on the server.');
// 	}

// 	if (!app) {
// 		app = initializeApp(firebaseConfig);
// 		auth = getAuth(app);
// 	}
// }

// const app = initializeApp(firebaseConfig);
// const auth = getAuth(app);

// export { app, auth };

// // export function getAuthWrapper() {
// // 	return getAuth(app);
// // }

// export async function register(request) {
// 	const data = await request.formData();

// 	const userCredential = await createUserWithEmailAndPassword(
// 		auth,
// 		data.get('email'),
// 		data.get('password')
// 	);

// 	const user = userCredential.user;

// 	await updateProfile(user, {
// 		displayName: data.get('name')
// 	});
// }

// export async function signIn(request) {
// 	const data = await request.formData();

// 	await signInWithEmailAndPassword(auth, data.get('email'), data.get('password'));
// }
