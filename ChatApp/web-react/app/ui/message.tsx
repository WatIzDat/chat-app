export default function Message({
    username,
    contents,
}: {
    username: string;
    contents: string;
}) {
    return (
        <div className="mt-8">
            <h4 className="font-bold underline">{username}</h4>
            <p>{contents}</p>
        </div>
    );
}
