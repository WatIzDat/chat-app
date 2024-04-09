import { initializeApp } from 'firebase/app';

const firebaseConfig = {
	apiKey: 'AIzaSyDjZtSoHT9oUphzi0tCMxrASftJMTS1NtI',
	authDomain: 'chat-app-59e5b.firebaseapp.com',
	projectId: 'chat-app-59e5b',
	storageBucket: 'chat-app-59e5b.appspot.com',
	messagingSenderId: '664884418847',
	appId: '1:664884418847:web:6a292a9dd524c3b58e382a'
};

const firebaseApp = initializeApp(firebaseConfig);

export async function handle({ event, resolve }) {
	event.locals.firebaseApp = firebaseApp;

	return await resolve(event);
}
