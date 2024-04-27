import { UserProfile } from "@clerk/nextjs";

export default function Page() {
    return (
        <div className="flex items-center justify-center h-screen">
            <UserProfile path="/user-profile" />
        </div>
    );
}
