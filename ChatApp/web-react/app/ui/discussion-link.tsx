import Link from "next/link";

export default function DiscussionLink({
    id,
    name,
}: {
    id: string;
    name: string;
}) {
    return (
        <Link
            className="flex items-center justify-center size-80 bg-zinc-900 rounded-3xl"
            href={`/discussion/${id}`}
        >
            {name}
        </Link>
    );
}
